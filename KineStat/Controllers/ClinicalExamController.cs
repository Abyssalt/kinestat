using KineStat.Data;
using KineStat.Filters;
using KineStat.Helpers;
using KineStat.Models;
using KineStat.Models.DTO;
using KineStat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;

namespace KineStat.Controllers
{
    [AuthorizePhysio]
    public class ClinicalExamController : Controller
    {
        private readonly KineDbContext _context;
        private readonly BayesService _bayesService;

        public ClinicalExamController(KineDbContext context)
        {
            _context = context;
            _bayesService = new BayesService(_context, new BayesCalculator());
        }

        /// <summary>
        /// Displays the clinical assessment view for a specified patient, folder, and assessment.
        /// </summary>
        /// <remarks>The user must have ownership of the specified patient to access this view. If the
        /// user does not have the required permissions, the method redirects to an access denied page.</remarks>
        /// <param name="id">The unique identifier of the patient whose clinical assessment is to be displayed.</param>
        /// <param name="folderId">The unique identifier of the folder containing the assessment.</param>
        /// <param name="assessmentId">The unique identifier of the clinical assessment to display.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the clinical assessment view if the current user has access;
        /// otherwise, redirects to an access denied page.</returns>
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
                        options = new[] { "Oui", "Non" },
                        sourceRv = question.SourceRv,           
                        rvPositive = question.RVPositive,       
                        rvNegative = question.RVNegative

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
                            .ToArray(),
                        sourceRv = question.SourceRv,           
                        rvPositive = question.RVPositive,      
                        rvNegative = question.RVNegative
                    };
                }
                else
                {
                    questionData = new
                    {
                        id = question.Id,
                        question = question.Title,
                        type = "text",
                        options = Array.Empty<string>(),
                        sourceRv = question.SourceRv,           
                        rvPositive = question.RVPositive,      
                        rvNegative = question.RVNegative
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

                var assessment = await _context.Assessments.FindAsync(dto.AssessmentId);
                if (assessment == null)
                {
                    return Json(new { success = false, message = "Assessment introuvable" });
                }

                foreach (var categoryId in examCliniqueCategories)
                {
                    var priorContext = await _context.PriorContexts
                        .FirstOrDefaultAsync(p => p.MedicalContextId == assessment.MedicalContextId
                                                  && p.CategoryId == categoryId);

                    if (priorContext == null || priorContext.Value <= 0 || priorContext.Value >= 1)
                    {
                        categoryScores.Add(0);
                        continue;
                    }

                    double categoryPrior = priorContext.Value;

                    try
                    {
                        double posterior = await _bayesService.CalculateClinicalCategoryProbability(
                            dto.PatientId,
                            dto.AssessmentId.Value,
                            categoryId,
                            categoryPrior
                        );

                        double radarValue = (posterior * 100) / 10 / 2;
                        categoryScores.Add(Math.Round(radarValue, 2));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur calcul catégorie {categoryId}: {ex.Message}");
                        categoryScores.Add(0);
                    }
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
        /// Retrieves the existing responses for a specified patient, optionally filtered by assessment.
        /// </summary>
        /// <remarks>The caller must be authorized to access the specified patient's data. Only responses
        /// to non-custom tests and questions not associated with a cluster are included.</remarks>
        /// <param name="id">The unique identifier of the patient whose responses are to be retrieved.</param>
        /// <param name="assessmentId">The optional identifier of the assessment to filter the responses. If null, responses from all assessments
        /// are included.</param>
        /// <returns>A JSON result containing a list of the patient's existing responses. Each response includes the question ID,
        /// response value, observations, and category name. Returns an empty list if no responses are found or if an
        /// error occurs.</returns>
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
        /// Retrieves the calculated percentage scores for each clinical category associated with a specific assessment
        /// for a given patient.
        /// </summary>
        /// <remarks>Access is restricted to physiotherapists who own the specified patient. The returned
        /// percentages represent normalized scores for predefined clinical categories within the assessment. If the
        /// assessment is not found or the user does not have access, an error message is returned.</remarks>
        /// <param name="patientId">The unique identifier of the patient whose clinical category percentages are to be retrieved.</param>
        /// <param name="assessmentId">The unique identifier of the assessment for which clinical category percentages are calculated.</param>
        /// <returns>A JSON result containing a success flag and an array of percentage scores for each clinical category if the
        /// operation succeeds; otherwise, a JSON result with an error message.</returns>

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

                var assessment = await _context.Assessments.FindAsync(assessmentId);
                if (assessment == null)
                {
                    return Json(new { success = false, message = "Assessment introuvable" });
                }

                foreach (var categoryId in examCliniqueCategories)
                {
                    var priorContext = await _context.PriorContexts
                        .FirstOrDefaultAsync(p => p.MedicalContextId == assessment.MedicalContextId
                                                  && p.CategoryId == categoryId);

                    if (priorContext == null || priorContext.Value <= 0 || priorContext.Value >= 1)
                    {
                        categoryScores.Add(0);
                        continue;
                    }

                    double categoryPrior = priorContext.Value;

                    try
                    {
                        double posterior = await _bayesService.CalculateClinicalCategoryProbability(
                            patientId,
                            assessmentId,
                            categoryId,
                            categoryPrior
                        );

                        double radarValue = (posterior * 100) / 10 / 2;
                        categoryScores.Add(Math.Round(radarValue, 2));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur calcul catégorie {categoryId}: {ex.Message}");
                        categoryScores.Add(0);
                    }
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
