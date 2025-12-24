using KineStat.Data;
using KineStat.Models;
using KineStat.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;
using KineStat.Filters;
using KineStat.Helpers;

namespace KineStat.Controllers
{
    [AuthorizePhysio]
    public class ClinicalExamController : Controller
    {
        private readonly KineDbContext _context;

        public ClinicalExamController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the clinical assessment view for the specified patient and assessment.
        /// </summary>
        /// <param name="id">The unique identifier of the patient whose clinical assessment is to be displayed.</param>
        /// <param name="assessmentId">The unique identifier of the clinical assessment to display for the patient.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the clinical assessment view for the specified patient and
        /// assessment.</returns>
        [Route("Patient/{id}/Folder/{folderId}/ExamenClinique/{assessmentId}")]
        public async Task<IActionResult> ExamenClinique(int id, int folderId, int assessmentId)
        {
            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, id))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            ViewData["AssessmentId"] = assessmentId.ToString();
            ViewData["PatientId"] = id.ToString();
            ViewData["FolderId"] = folderId.ToString();
            return View();
        }

        /// <summary>
        /// Retrieves a grouped list of clinical questions that are not associated with any cluster, organized by
        /// category.
        /// </summary>
        /// <remarks>Questions are grouped by their category name. If a question does not have an
        /// associated category, it is grouped under "Autre". The returned question types include boolean, multiple
        /// choice (QCM), ladder, and text, each with corresponding options. The method performs asynchronous database
        /// queries and is intended for use in HTTP GET requests.</remarks>
        /// <param name="id">The unique identifier of the clinic for which to retrieve questions.</param>
        /// <returns>A JSON result containing a dictionary where each key is a category name and each value is a list of
        /// questions belonging to that category. Each question includes its identifier, title, type, and available
        /// options.</returns>
        [HttpGet]
        public async Task<IActionResult> GetQuestionsClinique(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            var physioId = int.Parse(userIdString);

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, id))
            {
                return Unauthorized();
            }

            var examCliniqueCategories = new[] { 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            var allQuestions = await _context.Questions
                .Include(q => q.Category)
                .Where(q => q.ClusterId == null && examCliniqueCategories.Contains(q.CategoryId))
                .OrderBy(q => q.CategoryId)
                .ThenBy(q => q.Title)
                .ToListAsync();

            var groupedQuestions = new Dictionary<string, List<object>>();

            foreach (var question in allQuestions)
            {
                var categoryName = question.Category?.Name ?? "Autre";

                if (!groupedQuestions.ContainsKey(categoryName))
                {
                    groupedQuestions[categoryName] = new List<object>();
                }

                object questionData;

                if (question is QuestionBool qBool)
                {
                    questionData = new
                    {
                        id = question.Id,
                        question = question.Title,
                        type = "bool",
                        options = new[] { "Oui", "Non" }
                    };
                }
                else if (question is QuestionLadder qLadder)
                {
                    questionData = new
                    {
                        id = question.Id,
                        question = question.Title,
                        type = "ladder",
                        options = Enumerable.Range(qLadder.Min, qLadder.Max - qLadder.Min + 1)
                                            .Select(i => i.ToString())
                                            .ToArray()
                    };
                }
                else
                {
                    questionData = new
                    {
                        id = question.Id,
                        question = question.Title,
                        type = "text",
                        options = Array.Empty<string>()
                    };
                }

                groupedQuestions[categoryName].Add(questionData);
            }

            var filteredQuestions = groupedQuestions
                .Where(kvp => kvp.Value.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return Json(filteredQuestions);
        }


        /// <summary>
        /// Saves or updates clinical examination responses for a patient for the current day.
        /// </summary>
        /// <remarks>This method processes only responses for questions that are not part of a cluster and
        /// are not custom tests. Existing responses for the same patient and question on the current day are updated;
        /// otherwise, new responses are created. The operation is performed asynchronously.</remarks>
        /// <param name="dto">An object containing the patient identifier and a collection of clinical examination responses to be saved
        /// or updated. Cannot be null.</param>
        /// <returns>A JSON result indicating whether the operation was successful, including counts of saved and updated
        /// responses. If an error occurs, the result contains an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> SaveExamenClinique([FromBody] SaveExamenCliniqueDTO dto)
        {
            try
            {
                var dateResponse = DateTime.UtcNow;

                var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

                if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, dto.PatientId))
                {
                    return Json(new { success = false, message = "Accès refusé" });
                }

                var query = _context.PatientAnswerTests
                    .Where(pr => pr.PatientId == dto.PatientId
                              && !pr.IsCustomTest);

                if (dto.AssessmentId.HasValue)
                {
                    query = query.Where(pr => pr.AssessmentId == dto.AssessmentId.Value);
                }

                var existingResponses = await query.ToListAsync();

                int savedCount = 0;
                int updatedCount = 0;

                foreach (var responseDto in dto.Responses)
                {
                    var question = await _context.Questions
                        .FirstOrDefaultAsync(q => q.Id == responseDto.QuestionId && q.ClusterId == null);

                    if (question == null)
                    {
                        continue;
                    }

                    var existingResponse = existingResponses
                        .FirstOrDefault(r => r.QuestionId == responseDto.QuestionId);

                    if (existingResponse != null)
                    {
                        existingResponse.ResponseValue = responseDto.Response;
                        existingResponse.Observations = responseDto.Notes;
                        existingResponse.DateResponse = dateResponse;

                        _context.PatientAnswerTests.Update(existingResponse);
                        updatedCount++;
                    }
                    else
                    {
                        var newResponse = new PatientAnswerTests()
                        {
                            PatientId = dto.PatientId,
                            AssessmentId = dto.AssessmentId, 
                            QuestionId = responseDto.QuestionId,
                            ResponseValue = responseDto.Response,
                            Observations = responseDto.Notes,
                            DateResponse = dateResponse,
                            IsCustomTest = false,
                            AnswerId = null
                        };

                        _context.PatientAnswerTests.Add(newResponse);
                        savedCount++;
                    }
                }

                await _context.SaveChangesAsync();

                var examCliniqueCategories = new[] { 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                var categoryScores = new List<double>();

                foreach (var categoryId in examCliniqueCategories)
                {
                    var responses = await _context.PatientAnswerTests
                        .Include(pat => pat.Question)
                        .Where(pat =>
                            pat.PatientId == dto.PatientId &&
                            pat.AssessmentId == dto.AssessmentId &&
                            pat.Question.CategoryId == categoryId &&
                            pat.Question.ClusterId == null &&
                            !pat.IsCustomTest)
                        .ToListAsync();

                    if (!responses.Any())
                    {
                        categoryScores.Add(0);
                        continue;
                    }


                    double totalScore = 0;
                    int validResponseCount = 0;

                    foreach (var response in responses)
                    {
                        var question = await _context.Questions.FindAsync(response.QuestionId);

                        if (question is QuestionBool)
                        {
                            if (response.ResponseValue.ToLower() == "oui" || response.ResponseValue.ToLower() == "true")
                            {
                                totalScore += 2;
                            }
                            validResponseCount++;
                        } else
                        {
                            if (!string.IsNullOrWhiteSpace(response.ResponseValue))
                            {
                                totalScore += 2;
                            }
                        }
                    }
                    categoryScores.Add(Math.Round(totalScore/2, 2));
                }

                if (dto.AssessmentId.HasValue)
                {
                    for (int i = 0; i < examCliniqueCategories.Length; i++)
                    {
                        int categoryId = examCliniqueCategories[i];
                        double categoryValue = categoryScores[i];

                        var existingData = await _context.ClinicalDatas
                            .FirstOrDefaultAsync(cd =>
                                cd.PatientId == dto.PatientId &&
                                cd.AssessmentId == dto.AssessmentId.Value &&
                                cd.CategoryId == categoryId);

                        if (existingData != null)
                        {
                            existingData.Value = categoryValue;
                            _context.ClinicalDatas.Update(existingData);
                        }
                        else
                        {
                            var newData = new ClinicalData
                            {
                                PatientId = dto.PatientId,
                                AssessmentId = dto.AssessmentId.Value,
                                CategoryId = categoryId,
                                Value = categoryValue
                            };
                            _context.ClinicalDatas.Add(newData);
                        }
                    }

                    await _context.SaveChangesAsync();
                
                }

                return Json(new
                {
                    success = true,
                    message = "Réponses enregistrées avec succès",
                    savedCount = savedCount,
                    updatedCount = updatedCount,
                    totalCount = savedCount + updatedCount,
                    clinicalCategories = categoryScores
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Erreur lors de l'enregistrement: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Retrieves the list of responses submitted by the specified patient for standard tests on the current day.
        /// </summary>
        /// <remarks>Only responses for standard tests (non-custom) and questions not associated with a
        /// cluster are included. The returned objects contain question identifiers, response values, observations, and
        /// category names.</remarks>
        /// <param name="id">The unique identifier of the patient whose responses are to be retrieved.</param>
        /// <returns>A JSON result containing a list of response objects for the patient. If no responses are found or an error
        [HttpGet]
        public async Task<IActionResult> GetExistingResponses(int id, int? assessmentId)
        {
            try
            {
                var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

                if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, id))
                {
                    return Unauthorized();
                }

                var query = _context.PatientAnswerTests
                    .Include(pr => pr.Question)
                    .ThenInclude(q => q.Category)
                    .Where(pr => pr.PatientId == id
                                 && !pr.IsCustomTest
                                 && pr.Question.ClusterId == null);

                if (assessmentId.HasValue)
                {
                    query = query.Where(pr => pr.AssessmentId == assessmentId.Value);
                }

                var existingResponses = await query
                    .OrderByDescending(pr => pr.DateResponse)
                    .ToListAsync();

                var formattedResponses = existingResponses.Select(pr => new
                {
                    questionId = pr.QuestionId,
                    responseValue = pr.ResponseValue,
                    observations = pr.Observations,
                    categoryName = pr.Question?.Category?.Name ?? "Autre"
                }).ToList();

                return Json(formattedResponses);
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }

        /// <summary>
        /// Retrieves the calculated percentages for the 9 clinical categories for a specific assessment.
        /// </summary>
        [HttpGet]
        [Route("Patient/{patientId}/Assessment/{assessmentId}/ClinicalCategoryPercentages")]
        public async Task<IActionResult> GetClinicalCategoryPercentages(int patientId, int assessmentId)
        {
            try
            {
                var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

                if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, patientId))
                {
                    return Json(new { success = false, message = "Accès refusé" });
                }

                var examCliniqueCategories = new[] { 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                var categoryScores = new List<double>();

                foreach (var categoryId in examCliniqueCategories)
                {
                    var responses = await _context.PatientAnswerTests
                        .Include(pat => pat.Question)
                        .Where(pat =>
                            pat.PatientId == patientId &&
                            pat.AssessmentId == assessmentId &&
                            pat.Question.CategoryId == categoryId &&
                            pat.Question.ClusterId == null &&
                            !pat.IsCustomTest)
                        .ToListAsync();

                    if (!responses.Any())
                    {
                        categoryScores.Add(0);
                        continue;
                    }

                    double totalScore = 0;
                    int validResponseCount = 0;

                    foreach (var response in responses)
                    {
                        var question = await _context.Questions.FindAsync(response.QuestionId);

                        if (question is QuestionBool)
                        {
                            if (response.ResponseValue.ToLower() == "oui" || response.ResponseValue.ToLower() == "true")
                            {
                                totalScore += 2;
                            }

                        } else
                        {
                            if (!string.IsNullOrWhiteSpace(response.ResponseValue))
                            {
                                totalScore += 2;
                            }
                        }
                    }
                    categoryScores.Add(Math.Round(totalScore/2, 2));
                }

                return Json(new { success = true, clinicalCategories = categoryScores });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
