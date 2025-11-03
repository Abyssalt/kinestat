using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KineStat.Controllers
{
    [Route("Patient/{id}/{action}")]
    public class PatientController : Controller
    {
        public IActionResult Anamnese(int id)
        {
            ViewData["PatientId"] = id.ToString();
            return View();
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
        public IActionResult GetQuestions(int patientId/* ,int categoryId*/)
        {
            //TODO : recuperer les questions via leur categorie TINTIV via ADO puis les convertir en JSON (il existe fonction Json pour faire ça)

            // Données simulées 
            var data = new List<dynamic>
        {
            new { Category = "Tumeur / Métastase", Question="Antécédent personnel de cancer (localisation/année)", ExpectedData="Oui / Non", RvPlus="14,7", RvMinus=(string)null },
            new { Category = "Tumeur / Métastase", Question="Perte de poids inexpliquée (>5% en 3–6 mois)", ExpectedData="Oui / Non", RvPlus="9,2", RvMinus=(string)null },
            new { Category = "Tumeur / Métastase", Question="Douleur nocturne sévère, réveillant le patient", ExpectedData="Oui / Non", RvPlus="33,25", RvMinus=(string)null },
            new { Category = "Tumeur / Métastase", Question="Douleur progressive sans amélioration malgré traitement (>4 semaines)", ExpectedData="Oui / Non", RvPlus="3,1", RvMinus="0,8" },

            new { Category = "Infection", Question="Fièvre ≥ 38 °C ou frissons récents", ExpectedData="Oui / Non", RvPlus="68,8", RvMinus=(string)null },
            new { Category = "Infection", Question="Signes systémiques d’infection (sueurs nocturnes, fatigue importante)", ExpectedData="Oui / Non", RvPlus="1,8", RvMinus="1" },
            new { Category = "Infection", Question="Antécédent récent d’infection ou chirurgie/épisiotomie", ExpectedData="Oui / Non", RvPlus="4", RvMinus="0,6" },
            new { Category = "Infection", Question="Immunodépression documentée (corticothérapie chronique)", ExpectedData="Oui / Non", RvPlus="48,5", RvMinus="0,8" },

            new { Category = "Neurologique", Question="Anesthésie en selle / hypoesthésie périnéale", ExpectedData="Oui / Non", RvPlus="1,7", RvMinus="0,7" },
            new { Category = "Neurologique", Question="Rétention urinaire récente ou incontinence fécale / urinaire.", ExpectedData="Oui / Non", RvPlus="2 à 8,7", RvMinus="0,6" },
            new { Category = "Neurologique", Question="Faiblesse motrice aiguë ou progressive des membres inférieurs (MRC ≤ 3) ou chute récente", ExpectedData="Oui / Non", RvPlus="9,4", RvMinus="0,1" },
            new { Category = "Neurologique", Question="Troubles de la marche rapides ou signes pyramidaux / trouble coordination", ExpectedData="Oui / Non", RvPlus="3", RvMinus="0,4" }
        };

            // Transformation en JSON structure en categorie directement
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

        public IActionResult Prior(int id)
        {
            ViewData["PatientId"] = id.ToString();
            return View();
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
    }
}