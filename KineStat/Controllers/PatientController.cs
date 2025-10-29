using Microsoft.AspNetCore.Mvc;

namespace KineStat.Controllers
{
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