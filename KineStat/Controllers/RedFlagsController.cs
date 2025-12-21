using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using KineStat.Filters;
using KineStat.Helpers;
using KineStat.Services;
using System.Threading.Tasks;

namespace KineStat.Controllers
{
    [AuthorizePhysio]

    public class RedFlagsController : Controller
    {
        private readonly KineDbContext _context;
        private readonly BayesService _bayesService;


        public RedFlagsController(KineDbContext context)
        {
            _context = context;
            _bayesService = new BayesService(_context, new BayesCalculator());
        }

        /// <summary>
        /// Displays the Red Flags view for a specified patient and assessment.
        /// </summary>
        /// <remarks>The patient and assessment identifiers are made available to the view through the
        /// <see cref="ViewData"/> dictionary for use in rendering or further processing.</remarks>
        /// <param name="id">The unique identifier of the patient whose Red Flags are to be displayed.</param>
        /// <param name="assessmentId">The unique identifier of the assessment associated with the Red Flags.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the Red Flags view for the specified patient and assessment.</returns>
        [Route("Patient/{id}/Dossier/{folderId}/RedFlags/{assessmentId}")]
        public IActionResult RedFlags(int id, int folderId, int assessmentId)
        {
            ViewData["PatientId"] = id.ToString();
            ViewData["AssessmentId"] = assessmentId.ToString();
            ViewData["FolderId"] = folderId.ToString();
            return View();
        }

        /// <summary>
        /// Processes and saves the red flags percentage for the specified assessment, then redirects to the assessment
        /// details view.
        /// </summary>
        /// <remarks>This action requires a valid anti-forgery token and is intended to be called via HTTP
        /// POST. The percentage value is parsed using invariant culture, and invalid or missing input is treated as
        /// zero.</remarks>
        /// <param name="assessmentId">The unique identifier of the assessment to update.</param>
        /// <param name="redFlagsPercentage">The red flags percentage value to save, represented as a string. Can use either a comma or period as the
        /// decimal separator. If null, empty, or invalid, defaults to 0.</param>
        /// <returns>A redirect to the assessment details view if the assessment is found; otherwise, a NotFound result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRedFlagsPercentage(int assessmentId, string redFlagsPercentage)
        {
            var assessment = await _context.Assessments.FindAsync(assessmentId);

            if (assessment == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(redFlagsPercentage))
                redFlagsPercentage = "0";

            redFlagsPercentage = redFlagsPercentage.Replace(',', '.');

            if (!double.TryParse(
                    redFlagsPercentage,
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out double value))
            {
                value = 0;
            }

            assessment.RedFlagsPercentage = value;
            assessment.Status = AssessmentStatus.Cloture;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "AssessmentDetails",
                "Assessments",
                new { id = assessment.Id }
            );
        }

        /// <summary>
        /// Retrieves the list of boolean red flag questions and the patient's most recent answers for a specified
        /// category.
        /// </summary>
        /// <remarks>This method returns only boolean-type questions and their corresponding answers from
        /// the patient's latest assessment in the specified category. If no assessment exists for the patient, or if
        /// the patient is not found, the method returns a NotFound result.</remarks>
        /// <param name="patientId">The unique identifier of the patient whose red flag questions and answers are to be retrieved.</param>
        /// <param name="categoryId">The unique identifier of the question category for which red flag questions and answers are requested.</param>
        /// <returns>A partial view containing a collection of question and answer view models for the specified patient and
        /// category. Returns a 404 Not Found response if the patient or their latest assessment does not exist.</returns>
        [HttpGet]
        [Route("RedFlags/{patientId}/Assessment/{assessmentId}/Questions/{categoryId}")]
        public async Task<IActionResult> GetRedFlagsQuestions(int patientId, int assessmentId, int categoryId)
        {
            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, patientId))
            {
                return Unauthorized();
            }

            var patient = _context.Patients.Find(patientId);
            if (patient == null) return NotFound();

            var boolQuestions = _context.Questions
                .OfType<QuestionBool>()
                .Where(q => q.CategoryId == categoryId)
                .ToList();

            var boolAnswers = _context.PatientAnswers
                .OfType<PatientAnswerBool>()
                .Where(a => a.PatientId == patientId && a.Question.CategoryId == categoryId && a.AssessmentId == assessmentId)
                .ToList();

            var questionAndAnswers = boolQuestions.Select(q => new QuestionPatientAnswerVM
                {
                    PatientId = patientId,
                    Question = q,
                    Answer = boolAnswers.FirstOrDefault(ba => ba.QuestionId == q.Id)
                })
                .ToList();

            return PartialView("_QuestionsPartial", questionAndAnswers);
        }


        /// <summary>
        /// Creates or updates a patient's answer to a specific assessment question and returns the updated red flags
        /// percentage for the assessment.
        /// </summary>
        /// <remarks>Returns a 404 response if the patient does not exist, or a 400 response if the
        /// patient has no dossier or assessment. Returns a 500 response for unexpected errors or database update
        /// failures. The method is intended to be called via HTTP POST with a valid answer DTO in the request
        /// body.</remarks>
        /// <param name="answerDto">An object containing the patient's ID, question ID, answer value, and optional comment. The patient and
        /// assessment referenced must exist; otherwise, the request will fail.</param>
        /// <returns>An IActionResult containing a success status and the updated red flags percentage if the operation succeeds;
        /// otherwise, an error response indicating the reason for failure.</returns>
        [HttpPost]
        [Route("Patient/SaveOrUpdateAnswer")]
        public async Task<IActionResult> SaveOrUpdateAnswer([FromBody] SavePatientAnswerDTO answerDto)
        {
            try
            {

                var patient = FindPatientById(answerDto.PatientId);

                var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

                if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, answerDto.PatientId))
                {
                    return StatusCode(403, new { success = false, message = "Accès refusé" });
                }

                if (patient == null)
                {
                    return NotFound(new { success = false, message = "Le patient n'existe pas" });
                }

                var dossier = _context.Dossiers
                    .Where(d => d.PatientId == answerDto.PatientId)
                    .OrderByDescending(d => d.DateOuverture)
                    .ThenByDescending(d => d.Id)
                    .FirstOrDefault();
                if (dossier == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Ce patient ne possède aucun dossier."
                    });
                }

                var assessment = await _context.Assessments
                    .FirstOrDefaultAsync(a => a.Id == answerDto.AssessmentId && a.PatientId == answerDto.PatientId);
                if (assessment == null)
                {
                    return StatusCode(400, new
                    {
                        success = false,
                        message = "Erreur aucun bilan existant"
                    });
                }
                var savedAnswer = _context.PatientAnswers
                    .OfType<PatientAnswerBool>()
                    .FirstOrDefault(a => a.PatientId == answerDto.PatientId && a.QuestionId == answerDto.QuestionId && a.AssessmentId == assessment.Id);
                if (savedAnswer == null)
                {
                    savedAnswer = new PatientAnswerBool { PatientId = answerDto.PatientId, QuestionId = answerDto.QuestionId, Value = answerDto.BoolValue.Value, Comment = answerDto.Comment, AssessmentId = assessment.Id };
                    _context.PatientAnswers.Add(savedAnswer);
                }
                else
                {
                    savedAnswer = savedAnswer as PatientAnswerBool;
                    if (answerDto.BoolValue != null)
                    {
                        savedAnswer.Value = answerDto.BoolValue.Value;

                    }
                    savedAnswer.Comment = answerDto.Comment;
                }


                await _context.SaveChangesAsync();

                double redflagsPercentage = await GetSumRedflagsPercentage(answerDto.PatientId, assessment.Id);

                var categoryPercentages = new List<double>();

                for (int categoryId = 1; categoryId <= 6; categoryId++)
                {
                    await _context.SaveChangesAsync();

                    double probability = await CalculateRedFlagCategory(answerDto.PatientId, assessment.Id, categoryId);
                    double radarValue = (probability * 100) / 10 /2;
                    categoryPercentages.Add(radarValue);
                }

                foreach (var (radarValue, index) in categoryPercentages.Select((v, i) => (v, i)))
                {
                    int categoryId = index + 1;

                    var existingData = _context.ClinicalDatas
                        .FirstOrDefault(cd =>
                            cd.PatientId == answerDto.PatientId && cd.AssessmentId == assessment.Id &&
                            cd.CategoryId == categoryId);

                    if (existingData == null)
                    {
                        _context.ClinicalDatas.Add(new ClinicalData
                        {
                            PatientId = answerDto.PatientId,
                            AssessmentId = assessment.Id,
                            CategoryId = categoryId,
                            Value = radarValue
                        });
                    }
                    else
                    {
                        existingData.AssessmentId = assessment.Id;
                        existingData.Value = radarValue;
                    }
                }
                await _context.SaveChangesAsync();

                return Ok(new { success = true, redflags = redflagsPercentage, categories = categoryPercentages });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { success = false, message = "Erreur lors de la sauvegarde en base", details = dbEx.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Une erreur inattendue est survenue", details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a patient record that matches the specified patient identifier.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient to locate. Must correspond to an existing patient record.</param>
        /// <returns>A <see cref="Patient"/> object representing the patient with the specified identifier, or <c>null</c> if no
        /// matching patient is found.</returns>
        private Patient FindPatientById(int patientId)
        {
            var patient = _context.Patients
                .Where(p => p.Id == patientId)
                .FirstOrDefault();
            return patient;
        }

        /// <summary>
        /// Calculates the total percentage of red flags across all relevant categories for a specified patient and
        /// assessment.
        /// </summary>
        /// <remarks>The calculation includes only categories with an identifier less than or equal to 6.
        /// The method performs asynchronous operations for each category and aggregates the results.</remarks>
        /// <param name="patientId">The unique identifier of the patient for whom the red flag percentage is calculated.</param>
        /// <param name="assessmentId">The unique identifier of the assessment associated with the patient.</param>
        /// <returns>A double value representing the sum of red flag percentages for the specified patient and assessment. The
        /// value is the aggregate across all applicable categories.</returns>
        public async Task<double> GetSumRedflagsPercentage(int patientId, int assessmentId)
        {
            var categoryIds = _context.Categories
                .Where(c => c.Id <= 6)
                .Select(c => c.Id)
                .ToList();
            double result = 0;
            foreach (var id in categoryIds)
            {
                result += await CalculateRedFlagCategory(patientId, assessmentId, id) * 100;

            }
            return result;
        }

        /// <summary>
        /// Calculates the posterior probability for a red flag category based on a patient's answers and the prior
        /// context for a specific assessment.
        /// </summary>
        /// <remarks>This method uses Bayesian probability to combine the patient's answers with the prior
        /// context for the category. Ensure that prior context values are properly configured and valid before calling
        /// this method.</remarks>
        /// <param name="patientId">The unique identifier of the patient whose answers are evaluated.</param>
        /// <param name="assessmentId">The unique identifier of the assessment associated with the patient's answers.</param>
        /// <param name="categoryId">The unique identifier of the category for which the red flag probability is calculated.</param>
        /// <returns>A double value representing the posterior probability of the specified red flag category for the patient in
        /// the given assessment.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no prior context is defined for the specified medical context and category, or if the prior value
        /// is not strictly between 0 and 1.</exception>
        private async Task<double> CalculateRedFlagCategory(int patientId, int assessmentId, int categoryId)
        { 
            var assessment = await _context.Assessments.FindAsync(assessmentId);
            if (assessment == null)
            {
                throw new Exception(
                    $"Le bilan {assessmentId} n'existe pas.");

            }

            var priorContext = _context.PriorContexts
                .FirstOrDefault(p => p.MedicalContextId == assessment.MedicalContextId && p.CategoryId == categoryId);

            if (priorContext == null)
            {
                throw new InvalidOperationException(
                    $"Aucun prior défini pour le contexte médical {assessment.MedicalContextId} et la catégorie {categoryId}");
            }

            if (priorContext.Value <= 0 || priorContext.Value >= 1)
            {
                throw new InvalidOperationException(
                    $"Le prior ({priorContext.Value}) doit être strictement entre 0 et 1.");
            }
            double categoryPrior = priorContext.Value;

            double posterior = await _bayesService.CalculateCategoryProbability(patientId, assessmentId, categoryId, categoryPrior);

            return posterior;
        }

        /// <summary>
        /// Retrieves the red flags percentages for all TINTIV categories (categories 1-6)
        /// Returns values scaled for radar chart (0-10 scale)
        /// </summary>
        [HttpGet]
        [Route("Patient/{patientId}/Assessment/{assessmentId}/CategoryPercentages")]
        public async Task<IActionResult> GetCategoryPercentages(int patientId, int assessmentId)
        {
            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, patientId))
            {
                return StatusCode(403, new { success = false, message = "Accès refusé" });
            }

            try
            {
                var hasAnswers = await _context.PatientAnswers
                    .OfType<PatientAnswerBool>()
                    .AnyAsync(a => a.PatientId == patientId && a.AssessmentId == assessmentId);

                if (!hasAnswers)
                {
                    return Ok(new
                    {
                        success = true,
                        categories = new List<double> { 0, 0, 0, 0, 0, 0 },
                        totalPercentage = 0.0
                    });
                }

                var categoryPercentages = new List<double>();

                for (int categoryId = 1; categoryId <= 6; categoryId++)
                {
                    double probability = await CalculateRedFlagCategory(patientId, assessmentId, categoryId);
                    double radarValue = (probability * 100) / 10 / 2;
                    categoryPercentages.Add(radarValue);
                }

                double totalPercentage = await GetSumRedflagsPercentage(patientId, assessmentId);

                return Ok(new { success = true, categories = categoryPercentages, totalPercentage =totalPercentage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erreur lors du calcul des catégories", details = ex.Message });
            }
        }
    }
}
