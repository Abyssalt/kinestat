using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KineStat.Controllers
{
    [Route("Patient/{id}/{action}")]
    public class PatientController : Controller
    {
        private readonly KineDbContext _context;
        
    
        public PatientController(KineDbContext context)
        {
            _context = context;
        }

        public IActionResult Anamnese(int id)
        {
            var patient = _context.Patients
                .Include(p => p.Dossiers)
                .FirstOrDefault(p => p.Id == id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        [HttpPost]
        public IActionResult SaveAnamnese()
        {
            return RedirectToAction("Anamnese", new { id = 1 });
        }

        public IActionResult RedFlags(int id)
        {
            ViewData["PatientId"] = id.ToString();
            return View();
        }


        [HttpGet]
        public IActionResult GetRedFlagsQuestions(int patientId, int categoryId)
        {
            ViewData["redflagsPercentage"] = 0.0;
            var patient = _context.Patients.Find(patientId);
            if (patient == null) return NotFound();
            var boolQuestions = _context.Questions
                            .OfType<QuestionBool>()
                            .Where (q => q.CategoryId == categoryId)
                            .ToList();
            var boolAnswers = _context.PatientAnswers
                .OfType<PatientAnswerBool>()
                .Where(a => a.PatientId == patientId && a.Question.CategoryId == categoryId)
                .ToList();
 
            var questionAndAnswers = boolQuestions.Select(q => new QuestionPatientAnswerVM
            {
                PatientId = patientId,
                Question = q,
                Answer = boolAnswers.FirstOrDefault(ba => ba.QuestionId == q.Id)
            })
            .ToList();
            
            return PartialView("_QuestionsPartial",questionAndAnswers);

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
        [Route("~/Patient/SaveOrUpdateAnswer")]
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
                    .Where(a => a.PatientId == answerDto.PatientId)
                    .OrderByDescending(a => a.Date)
                    .FirstOrDefault();
                if (assessment == null)
                {
                    assessment = new Assessment
                    {
                        PatientId = answerDto.PatientId,
                        Date = DateTime.Now,
                        PhysioId = patient.PhysioId,
                        DossierId = dossier.Id
                    };
                    _context.Assessments.Add(assessment);
                    await _context.SaveChangesAsync();
                }
                var savedAnswer = _context.PatientAnswers
                    .OfType<PatientAnswerBool>()
                    .FirstOrDefault(a => a.PatientId == answerDto.PatientId && a.QuestionId == answerDto.QuestionId);
                if (savedAnswer == null)
                {
                    savedAnswer = new PatientAnswerBool { PatientId = answerDto.PatientId, QuestionId = answerDto.QuestionId, Comment = answerDto.Comment, AssessmentId = assessment.Id };
                    _context.PatientAnswers.Add(savedAnswer);
                } else
                {
                    savedAnswer = savedAnswer as PatientAnswerBool;
                    if (answerDto.BoolValue != null)
                    {
                        savedAnswer.Value = answerDto.BoolValue.Value;

                    }
                    savedAnswer.Comment = answerDto.Comment;
                }
                
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
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

        public IActionResult GetQuestionsClinique(int patientId/* ,int categoryId*/)
        {
            var data = new List<dynamic>
{
            new { Category = "Articulaire/structurel", Question = "Présence de raideur articulaire matinale (>30 minutes) ?", ExpectedData = "Oui / Non", RvPlus = "3,2", RvMinus = "0,5" },
            new { Category = "Articulaire/structurel", Question = "Mobilité articulaire réduite ou limitation de l’amplitude de mouvement ?", ExpectedData = "Oui / Non", RvPlus = "2,1", RvMinus = "0,7" },

            new { Category = "Myofascial", Question = "Présence de points gâchettes (trigger points) reproduisant la douleur ?", ExpectedData = "Oui / Non", RvPlus = "5,4", RvMinus = "0,6" },
            new { Category = "Myofascial", Question = "Douleur augmentée à la palpation ou lors de la contraction du muscle concerné ?", ExpectedData = "Oui / Non", RvPlus = "4,2", RvMinus = "0,8" },

            new { Category = "Nociceptif", Question = "La douleur est localisée et proportionnelle au mouvement ou à la charge ?", ExpectedData = "Oui / Non", RvPlus = "6,0", RvMinus = "0,4" },
            new { Category = "Nociceptif", Question = "Absence de symptômes neurologiques (fourmillements, engourdissements) ?", ExpectedData = "Oui / Non", RvPlus = "3,8", RvMinus = "0,5" },

            new { Category = "Neuropathique", Question = "Présence de picotements, brûlures ou sensations électriques ?", ExpectedData = "Oui / Non", RvPlus = "7,5", RvMinus = "0,3" },
            new { Category = "Neuropathique", Question = "Douleur suivant un territoire nerveux spécifique ?", ExpectedData = "Oui / Non", RvPlus = "5,1", RvMinus = "0,6" },

            new { Category = "Nociplastique", Question = "Douleur diffuse, non proportionnelle à la lésion tissulaire ?", ExpectedData = "Oui / Non", RvPlus = "8,3", RvMinus = "0,4" },
            new { Category = "Nociplastique", Question = "Sommeil non réparateur, fatigue persistante ou hypersensibilité généralisée ?", ExpectedData = "Oui / Non", RvPlus = "4,7", RvMinus = "0,6" },

            new { Category = "Contrôle sensorimoteur", Question = "Présence d’altération du schéma corporel ou de la perception du mouvement ?", ExpectedData = "Oui / Non", RvPlus = "3,5", RvMinus = "0,8" },
            new { Category = "Contrôle sensorimoteur", Question = "Difficulté à contrôler le mouvement ou à stabiliser le segment corporel ?", ExpectedData = "Oui / Non", RvPlus = "2,9", RvMinus = "0,7" },

            new { Category = "Croyances et cognition", Question = "Le patient pense que la douleur signifie nécessairement une lésion grave ?", ExpectedData = "Oui / Non", RvPlus = "2,4", RvMinus = "0,9" },
            new { Category = "Croyances et cognition", Question = "Le patient évite certaines activités par peur d’aggraver sa douleur ?", ExpectedData = "Oui / Non", RvPlus = "4,6", RvMinus = "0,7" },

            new { Category = "Socio-environnemental", Question = "Présence de facteurs de stress professionnel ou familial importants ?", ExpectedData = "Oui / Non", RvPlus = "3,1", RvMinus = "0,8" },
            new { Category = "Socio-environnemental", Question = "Soutien social limité ou isolement du patient ?", ExpectedData = "Oui / Non", RvPlus = "2,8", RvMinus = "0,9" },

            new { Category = "Émotionnel/affectif", Question = "Présence de symptômes d’anxiété, d’irritabilité ou de tristesse ?", ExpectedData = "Oui / Non", RvPlus = "3,9", RvMinus = "0,7" },
            new { Category = "Émotionnel/affectif", Question = "La douleur varie selon l’état émotionnel du patient ?", ExpectedData = "Oui / Non", RvPlus = "4,1", RvMinus = "0,6" }
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

        public IActionResult ExamenClinique(int id)
        {
            ViewData["PatientId"] = id.ToString();
            return View();
        }

        public IActionResult Tests(int id)
        {
            ViewData["PatientId"] = id.ToString();
            return View();
        }

        public IActionResult Resultat(int id)
        {
            ViewData["PatientId"] = id.ToString();
            return View();
        }



        [HttpGet]
        public IActionResult CreateDossier(int id)
        {
            return View(new Dossier { PatientId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDossier(Dossier dossier)
        {
            if (!ModelState.IsValid)
                return View(dossier);

            _context.Dossiers.Add(dossier);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✅ Dossier créé avec succès";

            return RedirectToAction("Anamnese", new { id = dossier.PatientId });
        }


        public async Task<IActionResult> DossierDetails(int id)
        {
            var dossier = await _context.Dossiers
                .Include(d => d.Patient)
                .Include(d => d.Assessments)
                .ThenInclude(a => a.Physio)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dossier == null)
                return NotFound();

            return View(dossier);
        }
        public IActionResult CreateAssessment(int dossierId)
        {
            return View(new Assessment
            {
                DossierId = dossierId,
                Date = DateTime.Today
            });
        }
        public async Task<IActionResult> AssessmentDetails(int id)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Patient)
                .Include(a => a.Physio)
                .Include(a => a.Dossier)
                .Include(a => a.RedFlagsDetected)
                .Include(a => a.Questions)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessment == null)
                return NotFound();

            return View(assessment);
        }


    }
}