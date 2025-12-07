using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace KineStat.Controllers
{
    public class AssessmentsController : Controller
    {
        private readonly KineDbContext _context;

        public AssessmentsController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Sends the view showing the details of an assessment.
        /// </summary>
        /// <param name="id">The Id of the assessment.</param>
        /// <returns>The view with the assessment's details.</returns>
        [Route("Assessment/{id}/Details")]
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

            var socrate = await _context.Set<Socrate>()
                .FirstOrDefaultAsync(s => s.AssessmentId == id);

            ViewBag.Socrate = socrate;
            var tintivData = await _context.ClinicalDatas
                .Where(cd => cd.PatientId == assessment.PatientId && cd.AssessmentId == assessment.Id && cd.CategoryId <= 6)
                .OrderBy(cd => cd.CategoryId)
                .Select(cd => cd.Value)
                .ToListAsync();

            ViewBag.TintivValues = tintivData;

            return View(assessment);
        }

        /// <summary>
        /// Sends the view for the SOCRATE questionnaire and start creating the assessment.
        /// </summary>
        /// <param name="id">The Id of the patient.</param>
        /// <param name="assessmentId">The Id of the assessment.</param>
        /// <returns>The view of the SOCRATE questionnaire.</returns>
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
                return RedirectToAction("Anamnese","Patient", new { id });
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

        /// <summary>
        /// Save the SOCRATE to the database.
        /// </summary>
        /// <param name="socrate">The SOCRATE questionnaire.</param>
        /// <returns>A redirection to the redflags page.</returns>
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

                return RedirectToAction("RedFlags", "RedFlags", new { id = socrate.PatientId, assessmentId = socrate.AssessmentId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de l'enregistrement : {ex.Message}";
                return RedirectToAction(nameof(Socrate), new { id = socrate.PatientId, assessmentId = socrate.AssessmentId });
            }
        }

        /// <summary>
        /// Delete an assessment.
        /// </summary>
        /// <param name="id">The Id of the assessment.</param>
        /// <param name="dossierId">The Id of the Folder.</param>
        /// <returns>A redirection to the folder's details.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int dossierId)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(
                "DossierDetails", "Folder",new { id = dossierId }
            );
        }

        /// <summary>
        /// Start a new assessment for a patient.
        /// </summary>
        /// <param name="PatientId">The Id of the patient.</param>
        /// <param name="DossierId">The Id of the folder.</param>
        /// <param name="PhysioId">The Id of the Physio.</param>
        /// <param name="MedicalContextId">The Id of the chosen medical context.</param>
        /// <returns>A redirection to the SOCRATE page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartAssessment(int PatientId, int DossierId, int PhysioId, int MedicalContextId)
        {
            var patient = await _context.Patients.FindAsync(PatientId);
            if (patient == null)
                return NotFound("Patient introuvable");

            var dossier = await _context.Dossiers.FindAsync(DossierId);
            if (dossier == null)
                return NotFound("Dossier introuvable");

            if (!await _context.MedicalContexts.AnyAsync(mc => mc.Id == MedicalContextId))
                return BadRequest("Contexte médical invalide");

            var assessment = new Assessment
            {
                PatientId = PatientId,
                PhysioId = PhysioId,
                DossierId = DossierId,
                MedicalContextId = MedicalContextId,
                Date = DateTime.Now
            };

            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Socrate", new { id = PatientId, assessmentId = assessment.Id });
        }

        /// <summary>
        /// Displays the assessment creation view for the specified dossier, pre-populated with patient and
        /// physiotherapist information.
        /// </summary>
        /// <remarks>The returned view is initialized with the current date and default values. If the
        /// dossier or its associated patient cannot be found, a NotFound result is returned. The list of available
        /// medical contexts is provided to the view via <c>ViewBag.MedicalContexts</c>.</remarks>
        /// <param name="dossierId">The unique identifier of the dossier for which the assessment is to be created. Must correspond to an
        /// existing dossier.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the assessment creation view if the dossier and patient exist;
        /// otherwise, a NotFound result indicating the missing resource.</returns>
        [Route("Dossier/{dossierId}/CreateAssessment")]
        public async Task<IActionResult> CreateAssessment(int dossierId)
        {
            var dossier = await _context.Dossiers
                .Include(d => d.Patient)
                .ThenInclude(p => p.Physio)
                .FirstOrDefaultAsync(d => d.Id == dossierId);

            if (dossier == null)
                return NotFound("Dossier introuvable");

            if (dossier.Patient == null)
                return NotFound("Patient introuvable");

            ViewBag.MedicalContexts = await _context.MedicalContexts.ToListAsync();

            return View(new Assessment
            {
                DossierId = dossierId,
                PatientId = dossier.Patient.Id,
                PhysioId = dossier.Patient.PhysioId,
                Date = DateTime.Today,
                RedFlagsPercentage = 0
            });
        }

        /// <summary>
        /// Returns a view displaying the assessment results for the specified patient and assessment identifiers.
        /// </summary>
        /// <param name="id">The unique identifier of the patient whose assessment is to be retrieved. Must match the patient associated
        /// with the assessment.</param>
        /// <param name="assessmentId">The unique identifier of the assessment to retrieve for the specified patient.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the assessment's results view if found and associated with the patient;
        /// otherwise, a NotFound or BadRequest result if the assessment does not exist or does not belong to the
        /// patient.</returns>
        [Route("Patient/{id}/Resultat/{assessmentId}")]
        public async Task<IActionResult> Resultat(int id, int assessmentId)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Patient)
                .Include(a => a.Dossier)
                .FirstOrDefaultAsync(a => a.Id == assessmentId);

            var tintivData = await _context.ClinicalDatas
                .Where(cd => cd.PatientId == assessment.PatientId
                             && cd.AssessmentId == assessment.Id
                             && cd.CategoryId <= 6)
                .OrderBy(cd => cd.CategoryId)
                .Select(cd => cd.Value)
                .ToListAsync();

            ViewBag.TintivValues = tintivData;


            if (assessment == null)
                return NotFound("Aucun bilan trouvé");

            if (assessment.PatientId != id)
                return BadRequest("Ce bilan n'appartient pas à ce patient.");
            ViewData["PatientId"] = id;
            ViewData["AssessmentId"] = assessment.Id;

            return View(assessment);
        }
    }
}