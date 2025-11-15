using KineStat.Models;
using KineStat.Data;
using Microsoft.AspNetCore.Mvc;
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
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
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
            return View(GetSimulatedQuestions());
        }

        //Private method used to get simulated 
        private List<QuestionBool> GetSimulatedQuestions()
        {
            var questions = new List<QuestionBool>
            {
                 // Tumeur / Métastase : Type = 1
                 new QuestionBool { Title = "Antécédent personnel de cancer (localisation/année)", RVPositif = 14.7, RVNegatif = 0, Type = "1" },
                 new QuestionBool { Title = "Perte de poids inexpliquée (>5% en 3–6 mois)", RVPositif = 9.2, RVNegatif = 0, Type = "1" },
                 new QuestionBool { Title = "Douleur nocturne sévère, réveillant le patient", RVPositif = 33.25, RVNegatif = 0, Type = "1" },
                 new QuestionBool { Title = "Douleur progressive sans amélioration malgré traitement (>4 semaines)", RVPositif = 3.1, RVNegatif = 0.8, Type = "1" },

                 // Infection  :Type = 2
                 new QuestionBool { Title = "Fièvre ≥ 38 °C ou frissons récents", RVPositif = 68.8, RVNegatif = 0, Type = "2" },
                 new QuestionBool { Title = "Signes systémiques d’infection (sueurs nocturnes, fatigue importante)", RVPositif = 1.8, RVNegatif = 1, Type = "2" },
                 new QuestionBool { Title = "Antécédent récent d’infection ou chirurgie/épisiotomie", RVPositif = 4, RVNegatif = 0.6, Type = "2" },
                 new QuestionBool { Title = "Immunodépression documentée (corticothérapie chronique)", RVPositif = 48.5, RVNegatif = 0.8, Type = "2" },

                 // Neurologique :  Type = 3
                 new QuestionBool { Title = "Anesthésie en selle / hypoesthésie périnéale", RVPositif = 1.7, RVNegatif = 0.7, Type = "3" },
                 new QuestionBool { Title = "Rétention urinaire récente ou incontinence fécale / urinaire.", RVPositif = 5.85, RVNegatif = 0.6, Type = "3" }, // moyenne 2 à 8,7
                 new QuestionBool { Title = "Faiblesse motrice aiguë ou progressive des membres inférieurs (MRC ≤ 3) ou chute récente", RVPositif = 9.4, RVNegatif = 0.1, Type = "3" },
                 new QuestionBool { Title = "Troubles de la marche rapides ou signes pyramidaux / trouble coordination", RVPositif = 3, RVNegatif = 0.4, Type = "3" }
             };

            return questions;
        }

        [HttpGet]
        public IActionResult GetRedFlagsQuestions(int patientId, int? categoryId)
        {
            ViewData["redflagsPercentage"] = 0.0;
            List<QuestionBool> questions = GetSimulatedQuestions();
            if (categoryId != null)
            {
                List<QuestionBool> filtredQuestions = new List<QuestionBool>();
                String category = categoryId.ToString();
                if (questions != null)
                {
                    filtredQuestions = questions
                        .Where(q => q.Type == category)
                        .ToList();
                    return PartialView("_QuestionsPartial", filtredQuestions);
                }

            }
            return PartialView("_QuestionsPartial", questions);
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
    }
}