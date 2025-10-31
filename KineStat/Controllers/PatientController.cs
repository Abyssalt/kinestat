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