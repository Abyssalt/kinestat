using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KineStat.Controllers
{
    public class PatientController : Controller
    {
        private readonly KineDbContext _context;
        private readonly BayesCalculator _bayesCalculator;
        public PatientController(KineDbContext context)
        {
            _context = context;
            _bayesCalculator = new BayesCalculator();
        }

        [Route("Patient/{id}/Anamnese")]
        public async Task<IActionResult> Anamnese(int id)
        {
            var patient = _context.Patients
                .Include(p => p.Dossiers)
                .FirstOrDefault(p => p.Id == id);

            if (patient == null)
                return NotFound();

            ViewBag.Physios = await _context.Physios
                .OrderBy(p => p.LastName)
                .ToListAsync();

            return View(patient);
        }

        [HttpPost]
        [Route("Patient/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingPatient = await _context.Patients.FindAsync(patient.Id);
                    if (existingPatient == null)
                    {
                        TempData["Error"] = "Patient introuvable.";
                        return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
                    }

                    existingPatient.FirstName = patient.FirstName;
                    existingPatient.LastName = patient.LastName;
                    existingPatient.Email = patient.Email;
                    existingPatient.PhoneNumber = patient.PhoneNumber;
                    existingPatient.Gender = patient.Gender;
                    existingPatient.Status = patient.Status;
                    existingPatient.BirthDate = patient.BirthDate;
                    existingPatient.SocialSecurityNumber = patient.SocialSecurityNumber;
                    existingPatient.PhysioId = patient.PhysioId;
                    existingPatient.Weight = patient.Weight;
                    existingPatient.Height = patient.Height;
                    existingPatient.DoctorName = patient.DoctorName;
                    existingPatient.DoctorINAMI = patient.DoctorINAMI;
                    existingPatient.Address = patient.Address;

                    _context.Update(existingPatient);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Patient {patient.FirstName} {patient.LastName} modifié avec succès.";
                    return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Erreur de validation : " + string.Join(", ", errors);
                return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Error"] = "Erreur de concurrence lors de la modification. Le patient a peut-être été modifié ou supprimé.";
                return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de la modification du patient : {ex.Message}";
                return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMedicalInfo(int PatientId, string? Profession, string? ActivitesPhysiques, string? AntecedentsMedicaux, string? MedicationActuelle)
        {
            var patient = await _context.Patients.FindAsync(PatientId);

            if (patient == null)
            {
                TempData["Error"] = "Patient introuvable";
                return RedirectToAction("Anamnese", new { id = PatientId });
            }

            patient.Profession = Profession;
            patient.ActivitesPhysiques = ActivitesPhysiques;
            patient.AntecedentsMedicaux = AntecedentsMedicaux;
            patient.MedicationActuelle = MedicationActuelle;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Informations médicales mises à jour avec succès";
            return RedirectToAction("Anamnese", new { id = PatientId });
        }


        [HttpPost]
        [Route("Patient/{id}/SaveAnamnese")]
        public IActionResult SaveAnamnese(int id)
        {
            return RedirectToAction("Anamnese", new { id = id });
        }

        [Route("Patient/{id}/Socrate/{assessmentId}")]
        public async Task<IActionResult> Socrate(int id, int assessmentId)
        {
            var patient = await _context.Patients.FindAsync(id);
            var assessment = await _context.Assessments.FindAsync(assessmentId);

            if (patient == null)
            {
                TempData["Error"] = "Patient introuvable.";
                return RedirectToAction("Index", "Patients");
            }

            if (assessment == null)
            {
                TempData["Error"] = "Bilan introuvable.";
                return RedirectToAction("Anamnese", new { id });
            }

            var socrate = await _context.Socrates
                .FirstOrDefaultAsync(s => s.AssessmentId == assessmentId);

            if (socrate == null)
            {
                socrate = new Socrate
                {
                    PatientId = id,
                    AssessmentId = assessmentId,
                };
            }

            ViewData["FolderId"] = assessment.DossierId;
            ViewData["AssessmentId"] = assessmentId;
            ViewBag.FirstName = patient.FirstName;
            ViewBag.LastName = patient.LastName;

            return View(socrate);
        }

        [HttpPost]
        [Route("Patient/SaveSocrate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSocrate(Socrate socrate)
        {
            try
            {
                bool hasAtLeastOneField = !string.IsNullOrWhiteSpace(socrate.Site) ||
                                          !string.IsNullOrWhiteSpace(socrate.Onset) ||
                                          !string.IsNullOrWhiteSpace(socrate.Character) ||
                                          !string.IsNullOrWhiteSpace(socrate.Radiation) ||
                                          !string.IsNullOrWhiteSpace(socrate.Association) ||
                                          !string.IsNullOrWhiteSpace(socrate.Timing) ||
                                          !string.IsNullOrWhiteSpace(socrate.ExacerbatingFactor) ||
                                          !string.IsNullOrWhiteSpace(socrate.RelievingFactor);

                if (!hasAtLeastOneField)
                {
                    TempData["Error"] = "Veuillez remplir au moins un champ du questionnaire SOCRATE.";
                    return RedirectToAction(nameof(Socrate), new { id = socrate.PatientId, assessmentId = socrate.AssessmentId });
                }

                var assessment = await _context.Assessments
                    .FirstOrDefaultAsync(a => a.Id == socrate.AssessmentId);

                if (assessment == null)
                {
                    TempData["Error"] = "Assessment introuvable.";
                    return RedirectToAction(nameof(Socrate), new { id = socrate.PatientId, assessmentId = socrate.AssessmentId });
                }

                var existingSocrate = await _context.Socrates
                    .FirstOrDefaultAsync(s => s.AssessmentId == socrate.AssessmentId);

                if (existingSocrate != null)
                {
                    existingSocrate.Site = socrate.Site;
                    existingSocrate.Onset = socrate.Onset;
                    existingSocrate.Character = socrate.Character;
                    existingSocrate.Radiation = socrate.Radiation;
                    existingSocrate.Association = socrate.Association;
                    existingSocrate.Timing = socrate.Timing;
                    existingSocrate.ExacerbatingFactor = socrate.ExacerbatingFactor;
                    existingSocrate.RelievingFactor = socrate.RelievingFactor;

                    _context.Update(existingSocrate);
                }
                else
                {
                    _context.Socrates.Add(socrate);
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = "Questionnaire SOCRATE enregistré avec succès.";

                return RedirectToAction(nameof(RedFlags), new { id = socrate.PatientId, assessmentId = assessment.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de l'enregistrement : {ex.Message}";
                return RedirectToAction(nameof(Socrate), new { id = socrate.PatientId, assessmentId = socrate.AssessmentId });
            }
        }


        [Route("Patient/{id}/RedFlags/{assessmentId}")]
        public IActionResult RedFlags(int id, int assessmentId)
        {
            ViewData["PatientId"] = id.ToString();
            ViewData["AssessmentId"] = assessmentId.ToString();
            return View();
        }


        [HttpGet]
        [Route("Patient/{patientId}/RedFlagsQuestions/{categoryId}")]
        public IActionResult GetRedFlagsQuestions(int patientId, int categoryId)
        {
            
            var patient = _context.Patients.Find(patientId);
            if (patient == null) return NotFound();
            var boolQuestions = _context.Questions
                            .OfType<QuestionBool>()
                            .Where(q => q.CategoryId == categoryId)
                            .ToList();
            var lastAssessment = _context.Assessments
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.Date)
                .ThenByDescending(a => a.Id)
                .FirstOrDefault();
            if (lastAssessment == null) return NotFound();
            var boolAnswers = _context.PatientAnswers
                .OfType<PatientAnswerBool>()
                .Where(a => a.PatientId == patientId && a.Question.CategoryId == categoryId && a.AssessmentId == lastAssessment.Id)
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

        //Return the patient in database with the specified Id 
        //if there is no patient return null
        private Patient FindPatientById(int patientId)
        {
            var patient = _context.Patients
                .Where(p => p.Id == patientId)
                .FirstOrDefault();
            return patient;
        }

        [HttpPost]
        [Route("Patient/SaveOrUpdateAnswer")]
        public async Task<IActionResult> SaveOrUpdateAnswer([FromBody] SavePatientAnswerDTO answerDto)
        {
            try
            {
              
                var patient = FindPatientById(answerDto.PatientId);
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

                var assessment = _context.Assessments
                    .Where(a => a.PatientId == answerDto.PatientId && a.DossierId == dossier.Id)
                    .OrderByDescending(a => a.Date)
                    .ThenByDescending(a => a.Id)
                    .FirstOrDefault();
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
  
                return Ok(new { success = true, redflags = redflagsPercentage});
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

        private async Task<Dictionary<int, double>> GetAllCategoriesProbability(int patientId, int assessmentId)
        {
            Dictionary<int, double> CategoryRedFlags = new Dictionary<int, double>();
            var categoryIds = _context.Categories
                .Select(c => c.Id)
                .ToList();
            foreach (var id in categoryIds)
            {
                double result = await CalculateRedFlagCategory(patientId, assessmentId, id);
                CategoryRedFlags[id] = result;
            }
            return CategoryRedFlags;
        }

        //This method return the sum off all redflags percentage from every category
        //This is not a probability but a sum of all percentage of every category used to indicate the level of redflags 
        public async Task<double> GetSumRedflagsPercentage(int patientId, int assessmentId) {
            var categoryIds = _context.Categories
                .Where (c=> c.Id <=6) 
                .Select(c => c.Id) 
                .ToList();
            double result = 0;
            foreach (var id in categoryIds)
            {
                result += await CalculateRedFlagCategory(patientId, assessmentId, id) * 100;
               
            }
            return result;
        }
        private async Task<double> CalculateRedFlagCategory(int patientId, int assessmentId, int categoryId)
        {
            var answersForCategory = _context.PatientAnswers
                .OfType<PatientAnswerBool>()
                .Where(a => a.PatientId == patientId && a.AssessmentId == assessmentId && a.Question.CategoryId == categoryId)
                .Include(a => a.Question)
                .ToList();

            var assessment = await  _context.Assessments.FindAsync(assessmentId);
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
            double posterior = _bayesCalculator.CalculateCategoryProbability(answersForCategory, categoryPrior);

            return posterior;
        }


        [Route("Patient/{id}/ExamenClinique/{assessmentId}")]
        public IActionResult ExamenClinique(int id, int assessmentId)
        {
            ViewData["AssessmentId"] = assessmentId.ToString();
            ViewData["PatientId"] = id.ToString();
            return View();
        }

        [Route("Patient/{id}/Tests/{assessmentId}")]
        public IActionResult Tests(int id, int assessmentId)
        {
            var clusters = _context.Cluster
                .Include(c => c.Questions)
                .ToList();

            ViewData["PatientId"] = id.ToString();
            ViewData["AssessmentId"] = assessmentId;

            return View(clusters); 
        }


        [Route("Patient/{id}/Resultat/{assessmentId}")]
        public async Task<IActionResult> Resultat(int id, int assessmentId)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Patient)
                .Include(a => a.Dossier)
                .FirstOrDefaultAsync(a => a.Id == assessmentId);

            if (assessment == null)
                return NotFound("Aucun bilan trouvé");

            if (assessment.PatientId != id)
                return BadRequest("Ce bilan n'appartient pas à ce patient.");

            ViewData["AssessmentId"] = assessment.Id;

            return View(assessment);
        }
    }
}