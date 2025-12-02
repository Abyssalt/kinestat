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
                var assessment = await _context.Assessments
                    .FirstOrDefaultAsync(a => a.Id == socrate.AssessmentId);

                if (assessment == null)
                {
                    TempData["Error"] = "Assessment introuvable.";
                    return RedirectToAction(nameof(Socrate), new { id = socrate.PatientId, assessmentId = assessment.Id });
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
                return RedirectToAction(nameof(Socrate), new { id = socrate.PatientId });
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

        [Route("Patient/{patientId}/QuestionsClinique")]
        public IActionResult GetQuestionsClinique(int patientId)
        {
            var data = new List<dynamic>
{
            new { Category = "Articulaire/structurel", Question = "Présence de raideur articulaire matinale (>30 minutes) ?", ExpectedData = "Oui / Non", RvPlus = "3,2", RvMinus = "0,5" },
            new { Category = "Articulaire/structurel", Question = "Mobilité articulaire réduite ou limitation de l'amplitude de mouvement ?", ExpectedData = "Oui / Non", RvPlus = "2,1", RvMinus = "0,7" },

            new { Category = "Myofascial", Question = "Présence de points gâchettes (trigger points) reproduisant la douleur ?", ExpectedData = "Oui / Non", RvPlus = "5,4", RvMinus = "0,6" },
            new { Category = "Myofascial", Question = "Douleur augmentée à la palpation ou lors de la contraction du muscle concerné ?", ExpectedData = "Oui / Non", RvPlus = "4,2", RvMinus = "0,8" },

            new { Category = "Nociceptif", Question = "La douleur est localisée et proportionnelle au mouvement ou à la charge ?", ExpectedData = "Oui / Non", RvPlus = "6,0", RvMinus = "0,4" },
            new { Category = "Nociceptif", Question = "Absence de symptômes neurologiques (fourmillements, engourdissements) ?", ExpectedData = "Oui / Non", RvPlus = "3,8", RvMinus = "0,5" },

            new { Category = "Neuropathique", Question = "Présence de picotements, brûlures ou sensations électriques ?", ExpectedData = "Oui / Non", RvPlus = "7,5", RvMinus = "0,3" },
            new { Category = "Neuropathique", Question = "Douleur suivant un territoire nerveux spécifique ?", ExpectedData = "Oui / Non", RvPlus = "5,1", RvMinus = "0,6" },

            new { Category = "Nociplastique", Question = "Douleur diffuse, non proportionnelle à la lésion tissulaire ?", ExpectedData = "Oui / Non", RvPlus = "8,3", RvMinus = "0,4" },
            new { Category = "Nociplastique", Question = "Sommeil non réparateur, fatigue persistante ou hypersensibilité généralisée ?", ExpectedData = "Oui / Non", RvPlus = "4,7", RvMinus = "0,6" },

            new { Category = "Contrôle sensorimoteur", Question = "Présence d'altération du schéma corporel ou de la perception du mouvement ?", ExpectedData = "Oui / Non", RvPlus = "3,5", RvMinus = "0,8" },
            new { Category = "Contrôle sensorimoteur", Question = "Difficulté à contrôler le mouvement ou à stabiliser le segment corporel ?", ExpectedData = "Oui / Non", RvPlus = "2,9", RvMinus = "0,7" },

            new { Category = "Croyances et cognition", Question = "Le patient pense que la douleur signifie nécessairement une lésion grave ?", ExpectedData = "Oui / Non", RvPlus = "2,4", RvMinus = "0,9" },
            new { Category = "Croyances et cognition", Question = "Le patient évite certaines activités par peur d'aggraver sa douleur ?", ExpectedData = "Oui / Non", RvPlus = "4,6", RvMinus = "0,7" },

            new { Category = "Socio-environnemental", Question = "Présence de facteurs de stress professionnel ou familial importants ?", ExpectedData = "Oui / Non", RvPlus = "3,1", RvMinus = "0,8" },
            new { Category = "Socio-environnemental", Question = "Soutien social limité ou isolement du patient ?", ExpectedData = "Oui / Non", RvPlus = "2,8", RvMinus = "0,9" },

            new { Category = "Émotionnel/affectif", Question = "Présence de symptômes d'anxiété, d'irritabilité ou de tristesse ?", ExpectedData = "Oui / Non", RvPlus = "3,9", RvMinus = "0,7" },
            new { Category = "Émotionnel/affectif", Question = "La douleur varie selon l'état émotionnel du patient ?", ExpectedData = "Oui / Non", RvPlus = "4,1", RvMinus = "0,6" }
        };


            var result = data
                .GroupBy(d => d.Category)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(q => new {
                        q.Question,
                        Type = q.Category,
                        q.ExpectedData,
                        Options = q.ExpectedData?.Split(" / ") ?? new string[0],
                        q.RvPlus,
                        q.RvMinus,
                        Notes = (string)null
                    }).ToList()
                );

            return Json(result);

        }

        [Route("Patient/{id}/ExamenClinique")]
        public IActionResult ExamenClinique(int id)
        {
            ViewData["PatientId"] = id.ToString();
            return View();
        }

        [Route("Patient/{id}/Tests")]
        public IActionResult Tests(int id)
        {
            ViewData["PatientId"] = id.ToString();
            return View();
        }


        [Route("Patient/{id}/Resultat")]
        public async Task<IActionResult> Resultat(int id)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Patient)
                .Include(a => a.Dossier)
                .OrderByDescending(a => a.Date)
                .FirstOrDefaultAsync(a => a.PatientId == id);

            if (assessment == null)
                return NotFound("Aucun bilan trouvé pour ce patient");

            ViewData["AssessmentId"] = assessment.Id;

            return View(assessment);
        }


    }
}