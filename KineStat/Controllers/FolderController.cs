using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KineStat.Filters;
using KineStat.Helpers;

namespace KineStat.Controllers
{
    [AuthorizePhysio]
    public class FolderController : Controller
    {
        private readonly KineDbContext _context;
        private readonly BayesCalculator _bayesCalculator;

        public FolderController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the view for creating a new folder associated with the specified patient.
        /// </summary>
        /// <param name="id">The unique identifier of the patient for whom the folder will be created.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the folder creation view for the specified patient.</returns>
        [HttpGet]
        [Route("Patient/{id}/CreateFolder")]
        public async Task<IActionResult> CreateFolder(int id)
        {
            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, id))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            return View(new Dossier { PatientId = id });
        }

        /// <summary>
        /// Creates a new patient folder by saving the specified dossier to the database.
        /// </summary>
        /// <remarks>This action requires a valid anti-forgery token and is intended to be called via HTTP
        /// POST. Success and error messages are provided through TempData for display in subsequent views.</remarks>
        /// <param name="dossier">The dossier containing information about the patient folder to be created. Must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> that redirects to the folder details view if the operation succeeds, or to
        /// the patient anamnesis view with an error message if the operation fails.</returns>
        [HttpPost]
        [Route("Patient/SaveFolder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveFolder(Dossier dossier)
        {
            dossier.DateOuverture = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);

            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, dossier.PatientId))
            {
                TempData["Error"] = "Vous n'avez pas accès à ce patient.";
                return RedirectToAction("Index", "Index");
            }

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
            return RedirectToAction("DossierDetails", "Folder", new { id = dossier.Id });
        }

        /// <summary>
        /// Displays detailed information for the dossier with the specified identifier.
        /// </summary>
        /// <remarks>The returned view includes related patient and assessment information. If no dossier
        /// exists with the specified identifier, a 404 Not Found response is returned.</remarks>
        /// <param name="id">The unique identifier of the dossier to retrieve. Must correspond to an existing dossier.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the dossier details view if the dossier is found; otherwise, a
        /// NotFound result.</returns>
        [Route("Dossier/{id}/Details")]
        public async Task<IActionResult> DossierDetails(int id)
        {
            var dossier = await _context.Dossiers
                .Include(d => d.Patient)
                .Include(d => d.Assessments)
                    .ThenInclude(a => a.Physio)
                .FirstOrDefaultAsync(d => d.Id == id);

            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsDossierOwnedByPhysio(_context, physioId, id))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            if (dossier == null)
                return NotFound();

            return View(dossier);
        }
    }
}