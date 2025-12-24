using KineStat.Data;
using KineStat.Models;
using KineStat.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KineStat.Filters;
using KineStat.Helpers;

namespace KineStat.Controllers
{
    [AuthorizePhysio]
    public class TestsController : Controller
    {
        private readonly KineDbContext _context;

        public TestsController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles HTTP GET requests to display the test clusters and latest test responses for a specified patient.
        /// </summary>
        /// <remarks>The returned view includes the patient's details, available test clusters with
        /// questions, and the most recent test responses. If no responses exist for the patient, the view will display
        /// only the available clusters. This action is intended for use within an ASP.NET MVC application and requires
        /// a valid patient identifier.</remarks>
        /// <param name="id">The unique identifier of the patient whose test information is to be retrieved.</param>
        /// <returns>An asynchronous operation that returns an <see cref="IActionResult"/> representing the rendered 'Tests' view
        /// with patient and test data.</returns>
        [HttpGet]
        [Route("Patient/{id}/Dossier/{folderId}/Tests/{assessmentId}")]
        public async Task<IActionResult> Tests(int id, int folderId, int assessmentId)
        {
            var patient = await _context.Patients
                .Include(p => p.Physio)
                .FirstOrDefaultAsync(p => p.Id == id);

            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, id))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            var allClusters = await _context.Cluster
                .Include(c => c.Questions)
                .ToListAsync();

            var clustersWithQuestions = allClusters
                .Where(c => c.Questions != null && c.Questions.Count > 0)
                .ToList();


            var today = DateTime.UtcNow.Date;

            var existingResponses = await _context.PatientAnswerTests
                .Where(pr => pr.PatientId == id && pr.AssessmentId == assessmentId)
                .ToListAsync();


            //var latestSessionDate = latestResponses.FirstOrDefault()?.DateResponse.Date;
            //if (latestSessionDate.HasValue)
            //{
            //  latestResponses = latestResponses
            //    .Where(pr => pr.DateResponse.Date == latestSessionDate.Value)
            //  .ToList();

            //}

            ViewData["Patient"] = patient;
            ViewData["PatientId"] = id;
            ViewData["AssessmentId"] = assessmentId;
            ViewData["PatientNom"] = $"{patient.FirstName} {patient.LastName}";
            ViewData["Clusters"] = clustersWithQuestions;
            ViewData["ExistingResponses"] = existingResponses;
            ViewData["FolderId"] = folderId;

            return View("Tests", clustersWithQuestions);
        }

        /// <summary>
        /// Saves or updates patient test results for the current day based on the provided data transfer object.
        /// </summary>
        /// <remarks>If a test result for the patient and the current day already exists, it will be
        /// updated; otherwise, a new result will be created. The response includes counts of new and updated test
        /// results, as well as the evaluation date. This method is intended to be called via HTTP POST with a JSON
        /// payload.</remarks>
        /// <param name="dto">An object containing the patient identifier and a collection of test results to be saved or updated. The
        /// patient identifier must be greater than zero.</param>
        /// <returns>An IActionResult indicating the outcome of the operation. Returns 200 OK with details of saved and updated
        /// tests if successful; 400 Bad Request if the patient identifier is invalid; 404 Not Found if the patient does
        /// not exist; or 500 Internal Server Error if an unexpected error occurs.</returns>
        [HttpPost]
        public async Task<IActionResult> SaveTestResults([FromBody] SaveTestResultsDTO dto)
        {
            try
            {
                if (dto.PatientId <= 0)
                {
                    return BadRequest(new { success = false, message = "Patient invalide" });
                }

                var userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
                var physioId = int.Parse(userIdString);

                if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, dto.PatientId))
                {
                    return StatusCode(403, new { success = false, message = "Accès refusé" });
                }

                var patient = await _context.Patients.FindAsync(dto.PatientId);
                if (patient == null)
                {
                    return NotFound(new { success = false, message = "Patient introuvable" });
                }

                var dateResponse = DateTime.UtcNow;

                var query = _context.PatientAnswerTests
                    .Where(pr => pr.PatientId == dto.PatientId);

                if (dto.AssessmentId.HasValue)
                {
                    query = query.Where(pr => pr.AssessmentId == dto.AssessmentId.Value);
                }

                var existingResponses = await query.ToListAsync();

                int savedCount = 0;
                int updatedCount = 0;

                foreach (var test in dto.Tests)
                {
                    if (string.IsNullOrWhiteSpace(test.Value) && string.IsNullOrWhiteSpace(test.Observations))
                    {
                        continue;
                    }

                    PatientAnswerTests response = null;

                    if (test.Custom)
                    {
                        response = existingResponses.FirstOrDefault(r =>
                            r.IsCustomTest &&
                            r.CustomTestName == test.Name);
                    }
                    else
                    {

                        response = existingResponses.FirstOrDefault(r =>
                            !r.IsCustomTest &&
                            r.QuestionId == test.Id);
                    }

                    if (response != null)
                    {
                        response.ResponseValue = test.Value;
                        response.Observations = test.Observations;
                        response.DateResponse = dateResponse; 

                        _context.PatientAnswerTests.Update(response);
                        updatedCount++;
                    }
                    else
                    {
                        response = new PatientAnswerTests
                        {
                            PatientId = dto.PatientId,
                            AssessmentId = dto.AssessmentId,
                            DateResponse = dateResponse,
                            ResponseValue = test.Value,
                            Observations = test.Observations,
                            IsCustomTest = test.Custom,
                            CustomTestName = test.Custom ? test.Name : null,
                            CustomTestType = test.Custom ? test.Type : null,
                            QuestionId = test.Custom ? null : test.Id,
                            AnswerId = null
                        };

                        _context.PatientAnswerTests.Add(response);
                        savedCount++;
                    }
                }

                if (savedCount > 0 || updatedCount > 0)
                {
                    await _context.SaveChangesAsync();
                }

                return Ok(new
                {
                    success = true,
                    message = "Résultats sauvegardés avec succès",
                    saved = savedCount,
                    updated = updatedCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Une erreur est survenue lors de la sauvegarde." });
            }
        }
    }
}
