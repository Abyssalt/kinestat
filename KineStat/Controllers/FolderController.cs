using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KineStat.Controllers
{
    public class FolderController : Controller
    {
        private readonly KineDbContext _context;
        private readonly BayesCalculator _bayesCalculator;

        public FolderController(KineDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Patient/{id}/CreateFolder")]
        public IActionResult CreateFolder(int id)
        {
            return View(new Dossier { PatientId = id });
        }

        [HttpPost]
        [Route("Patient/SaveDossier")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDossier(Dossier dossier)
        {
            dossier.DateOuverture = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
            _context.Dossiers.Add(dossier);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur: {ex.InnerException?.Message ?? ex.Message}";
                return RedirectToAction("Anamnese", "Patient", new { id = dossier.PatientId });
            }

            TempData["Success"] = "Dossier créé avec succès";
            return RedirectToAction("Anamnese", "Patient", new { id = dossier.PatientId });
        }

        [Route("Dossier/{id}/Details")]
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
    }
}