using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KineStat.Filters;
using KineStat.Helpers;
using KineStat.Models.DTO;
using KineStat.Models.ViewModels;

namespace KineStat.Controllers
{
    [AuthorizePhysio]
    public class AssessmentsController : Controller
    {
        private readonly KineDbContext _context;
        private readonly BayesService _bayesService;

        public AssessmentsController(KineDbContext context)
        {
            _context = context;
            _bayesService = new BayesService(context, new BayesCalculator());   
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

            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsAssessmentOwnedByPhysio(_context, physioId, id))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

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

            var clinicalData = await _context.ClinicalDatas
                .Where(cd => cd.PatientId == assessment.PatientId && cd.AssessmentId == assessment.Id && cd.CategoryId >= 7 && cd.CategoryId <= 15)
                .OrderBy(cd => cd.CategoryId)
                .Select(cd => cd.Value)
                .ToListAsync();

            ViewBag.ClinicalValues = clinicalData;

            var tests = await _context.PatientAnswerTests
                .Include(t => t.Question)
                .ThenInclude(q => q.Cluster)
                .Where(t =>
                    t.PatientId == assessment.PatientId &&
                    t.AssessmentId == assessment.Id &&
                    (
                        t.IsCustomTest ||
                        (t.Question != null && t.Question.Cluster != null)
                    )
                )
                .OrderBy(t => t.IsCustomTest
                    ? "Tests personnalisés"
                    : t.Question!.Cluster!.Name)
                .ThenBy(t => t.DateResponse)
                .ToListAsync();

            ViewBag.Tests = tests;

            ViewBag.TintivValues = tintivData;
            var firstAssessment = await _context.Assessments
                .Where(a => a.DossierId == assessment.DossierId)
                .OrderBy(a => a.Date)
                .Select(a => new { a.Id })
                .FirstOrDefaultAsync();

            List<double>? firstTintivData = null;

            if (firstAssessment != null && firstAssessment.Id != assessment.Id)
            {
                firstTintivData = await _context.ClinicalDatas
                    .Where(cd =>
                        cd.AssessmentId == firstAssessment.Id &&
                        cd.CategoryId>=1 && cd.CategoryId <= 15)
                    .OrderBy(cd => cd.CategoryId)
                    .Select(cd => cd.Value)
                    .ToListAsync();
            }

            ViewBag.FirstTintivValues = firstTintivData;

            var detectedPathologiesProbabilities = await _context.PatientPathologiesDetecteds
             .Include(p => p.Pathology)
             .Where(p => p.PatientId == assessment.PatientId
                && p.AssessmentId == assessment.Id)
             .ToListAsync();

            ViewBag.DetectedPathologies = detectedPathologiesProbabilities;


            var redFlagsAnswers = await _context.PatientAnswers
                .OfType<PatientAnswerBool>()
                .Include(a => a.Question)
                .ThenInclude(q => q.Category)
                .Where(a => a.PatientId == assessment.PatientId && a.AssessmentId == assessment.Id)
                .OrderBy(a => a.Question.CategoryId)
                .ToListAsync();

            ViewBag.RedFlagsAnswers = redFlagsAnswers;

            var clinicalAnswers = await _context.PatientAnswerTests
                .Include(t => t.Question)
                .ThenInclude(q => q.Category)
                .Where(t => t.PatientId == assessment.PatientId
                            && t.AssessmentId == assessment.Id
                            && t.Question.ClusterId == null
                            && !t.IsCustomTest
                            && t.Question.CategoryId >= 7
                            && t.Question.CategoryId <= 15)
                .OrderBy(t => t.Question.CategoryId)
                .ToListAsync();

            ViewBag.ClinicalAnswers = clinicalAnswers;

            var otherAssessments = await _context.Assessments
                .Where(a => a.DossierId == assessment.DossierId
                            && a.Id != assessment.Id
                            && (firstAssessment == null || a.Id != firstAssessment.Id))
                .OrderBy(a => a.Date)
                .Select(a => new { a.Id, a.Date })
                .ToListAsync();

            ViewBag.OtherAssessments = otherAssessments;


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

            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, id))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            if (patient == null)
            {
                TempData["Error"] = "Patient introuvable.";
                return RedirectToAction("Index", "Index");
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

                return RedirectToAction("RedFlags", "RedFlags", new { id = socrate.PatientId, folderId=assessment.DossierId ,assessmentId = socrate.AssessmentId });
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

            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsAssessmentOwnedByPhysio(_context, physioId, id))
            {
                TempData["Error"] = "Vous n'avez pas accès à ce bilan.";
                return RedirectToAction("Index", "Index");
            }

            if (assessment != null)
            {
                var clinicalDatas = _context.ClinicalDatas.Where(cd => cd.AssessmentId == id);
                _context.ClinicalDatas.RemoveRange(clinicalDatas);

                var socrate = await _context.Socrates.FirstOrDefaultAsync(s => s.AssessmentId == id);
                if (socrate != null)
                {
                    _context.Socrates.Remove(socrate);
                }

                var testAnswers = _context.PatientAnswerTests.Where(t => t.AssessmentId == id);
                _context.PatientAnswerTests.RemoveRange(testAnswers);

                _context.Assessments.Remove(assessment);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(
                "DossierDetails", "Folder", new { id = dossierId }
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

            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, PatientId))
            {
                TempData["Error"] = "Vous n'avez pas accès à ce patient.";
                return RedirectToAction("Index", "Index");
            }

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

            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsDossierOwnedByPhysio(_context, physioId, dossierId))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

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
                RedFlagsPercentage = 0,
                Status = AssessmentStatus.EnCours
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
        [Route("Patient/{id}/Dossier/{folderId}/Resultat/{assessmentId}")]
        public async Task<IActionResult> Resultat(int id,int folderId, int assessmentId)
        {
            var assessment = await _context.Assessments
                .Include(a => a.Patient)
                .Include(a => a.Dossier)
                .FirstOrDefaultAsync(a => a.Id == assessmentId);

            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsAssessmentOwnedByPhysio(_context, physioId, assessmentId))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            var tintivData = await _context.ClinicalDatas
                .Where(cd => cd.PatientId == assessment.PatientId
                             && cd.AssessmentId == assessment.Id
                             && cd.CategoryId <= 6)
                .OrderBy(cd => cd.CategoryId)
                .Select(cd => cd.Value)
                .ToListAsync();

            ViewBag.TintivValues = tintivData;

            var clinicalData = await _context.ClinicalDatas
                .Where(cd => cd.PatientId == assessment.PatientId
                             && cd.AssessmentId == assessment.Id
                             && cd.CategoryId >= 7
                             && cd.CategoryId <= 15)
                .OrderBy(cd => cd.CategoryId)
                .Select(cd => cd.Value)
                .ToListAsync();

            ViewBag.ClinicalValues = clinicalData;

            var tests = await _context.PatientAnswerTests
                .Include(t => t.Question)
                .ThenInclude(q=> q.Cluster)
                .Where(t =>
                    t.PatientId == id &&
                    t.AssessmentId == assessmentId &&
                    (
                        t.IsCustomTest ||
                        (t.Question != null && t.Question.Cluster != null)
                    )
                )

                .OrderBy(t => t.DateResponse)
                .OrderBy(t => t.Question.Cluster.Name)
                .ToListAsync();

            ViewBag.Tests = tests;

            var firstAssessment = await _context.Assessments
                .Where(a => a.DossierId == assessment.DossierId)
                .OrderBy(a => a.Date)
                .Select(a => new { a.Id })
                .FirstOrDefaultAsync();

            List<double>? firstTintivData = null;

            if (firstAssessment != null && firstAssessment.Id != assessment.Id)
            {
                firstTintivData = await _context.ClinicalDatas
                    .Where(cd =>
                        cd.AssessmentId == firstAssessment.Id &&
                        cd.CategoryId <= 6)
                    .OrderBy(cd => cd.CategoryId)
                    .Select(cd => cd.Value)
                    .ToListAsync();
            }

            ViewBag.FirstTintivValues = firstTintivData;


            if (assessment == null)
                return NotFound("Aucun bilan trouvé");

            if (assessment.PatientId != id)
                return BadRequest("Ce bilan n'appartient pas à ce patient.");
            ViewData["PatientId"] = id;
            ViewData["AssessmentId"] = assessment.Id;
            ViewData["FolderId"] = folderId;
            var detectedPathologies = await CalculateAndSaveDetectedPathologies(
                patientId: id,
                assessmentId: assessmentId,
                folderId: folderId
             );

            
            ViewBag.DetectedPathologies = detectedPathologies;

            return View(assessment);
        }

        /// <summary>
        /// Calculates and saves the detected pathologies for a patient assessment,
        /// considering only the pathologies actually present in the patient's answers.
        /// </summary>
        private async Task<List<DetectedPathologyDTO>> CalculateAndSaveDetectedPathologies(int patientId, int assessmentId, int folderId)
        {
            var assessment = await _context.Assessments
                .Where(a => a.Id == assessmentId && a.DossierId == folderId)
                .Include(a => a.Patient)
                .Include(a => a.Dossier)
                .FirstOrDefaultAsync();
            if (assessment == null) throw new Exception("Erreur bilan inexistant");

            var priorContext = await _context.PriorContexts
                .Where(m => m.MedicalContextId == assessment.MedicalContextId)
                .FirstOrDefaultAsync();
            if (priorContext == null) throw new Exception("Erreur prior inexistant pour ce contexte");

            var priorProbability = priorContext.Value;

            var detectedPathologies = new List<DetectedPathologyDTO>();



            // Take PathologiesId in PatientAnswers
            var patientPathologyIds = await _context.PatientAnswers
                .Where(a => a.PatientId == patientId && a.AssessmentId == assessmentId && a.Assessment.DossierId == folderId)
                .SelectMany(a => a.Question.QuestionPathologies.Select(qp => qp.PathologyId))
                .Distinct()
                .ToListAsync();

            // Take all pathologies linked to Patient's pathologies
            var relevantPathologies = await _context.Pathologies
                .Where(p => patientPathologyIds.Contains(p.Id))
                .ToListAsync();


            //For every patient detected pathologie calculate pathology probability
            foreach (var pathology in relevantPathologies)
            {
              
                double probability = await _bayesService.CalculateProbabilityByPathology(
                    prior: priorProbability,
                    patientId: patientId,
                    assessmentId: assessmentId,
                    folderId : folderId,
                    pathologyId: pathology.Id
                );

                var dto = new DetectedPathologyDTO
                {
                    PatientId = patientId,
                    AssessmentId = assessmentId,
                    PathologyName = pathology.Name,
                    PathologyPercentage = probability * 100
                };
                detectedPathologies.Add(dto);

              
                var existing = await _context.PatientPathologiesDetecteds
                    .FirstOrDefaultAsync(ppd =>
                        ppd.PatientId == patientId &&
                        ppd.AssessmentId == assessmentId &&
                        ppd.PathologyId == pathology.Id

                    );

                if (existing != null)
                {
                    existing.PathologyProbability = probability;
                    _context.PatientPathologiesDetecteds.Update(existing);
                }
                else
                {
                    var newEntry = new PatientPathologiesDetected
                    {
                        PatientId = patientId,
                        AssessmentId = assessmentId,
                        PathologyId = pathology.Id,
                        PathologyProbability = probability,
                        
                    };
                    await _context.PatientPathologiesDetecteds.AddAsync(newEntry);
                }
            }

            await _context.SaveChangesAsync();

            return detectedPathologies;
        }


        /// Get comparison data for a specific assessment (TINTIV and Clinical values).
        /// </summary>
        /// <param name="id">The Id of the assessment to compare.</param>
        /// <returns>JSON data with assessment date and values.</returns>
        [Route("Assessment/{id}/ComparisonData")]
        public async Task<IActionResult> GetComparisonData(int id)
        {
            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsAssessmentOwnedByPhysio(_context, physioId, id))
            {
                return Forbid();
            }

            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment == null)
                return NotFound();

            var tintivData = await _context.ClinicalDatas
                .Where(cd => cd.AssessmentId == id && cd.CategoryId <= 6)
                .OrderBy(cd => cd.CategoryId)
                .Select(cd => cd.Value)
                .ToListAsync();

            var clinicalData = await _context.ClinicalDatas
                .Where(cd => cd.AssessmentId == id && cd.CategoryId >= 7 && cd.CategoryId <= 15)
                .OrderBy(cd => cd.CategoryId)
                .Select(cd => cd.Value)
                .ToListAsync();

            return Json(new
            {
                date = assessment.Date.ToString("dd/MM/yyyy"),
                tintivValues = tintivData,
                clinicalValues = clinicalData
            });
        }

        /// <summary>
        /// Exports the complete assessment report (Anamnesis, SOCRATE, Results) to PDF format from Assessment Details page.
        /// </summary>
        /// <param name="id">The assessment ID.</param>
        /// <returns>A PDF file download.</returns>
        [Route("Assessment/{id}/ExportPdf")]
        public async Task<IActionResult> ExportAssessmentDetailsPdf(int id)
        {
            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsAssessmentOwnedByPhysio(_context, physioId, id))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            // Getting assessment with all its relations
            var assessment = await _context.Assessments
                .Include(a => a.Patient)
                .Include(a => a.Dossier)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessment == null)
            {
                TempData["Error"] = "Bilan introuvable.";
                return RedirectToAction("AssessmentDetails", new { id });
            }

            var patient = assessment.Patient;

            // Getting Socrate
            var socrate = await _context.Socrates
                .FirstOrDefaultAsync(s => s.AssessmentId == id);

            // Getting tests
            var tests = await _context.PatientAnswerTests
                .Include(t => t.Question)
                .ThenInclude(q => q.Cluster)
                .Where(t =>
                    t.PatientId == patient.Id &&
                    t.AssessmentId == id &&
                    (
                        t.IsCustomTest ||
                        (t.Question != null && t.Question.Cluster != null)
                    )
                )
                .OrderBy(t => t.IsCustomTest
                    ? "Tests personnalisés"
                    : t.Question!.Cluster!.Name)
                .ThenBy(t => t.DateResponse)
                .ToListAsync();

            // Getting Tintiv data
            var tintivData = await _context.ClinicalDatas
                .Where(cd => cd.PatientId == patient.Id
                             && cd.AssessmentId == id
                             && cd.CategoryId <= 6)
                .OrderBy(cd => cd.CategoryId)
                .Select(cd => cd.Value)
                .ToListAsync();

            // Getting clinical data
            var clinicalData = await _context.ClinicalDatas
                .Where(cd => cd.PatientId == patient.Id
                             && cd.AssessmentId == id
                             && cd.CategoryId >= 7
                             && cd.CategoryId <= 15)
                .OrderBy(cd => cd.CategoryId)
                .Select(cd => cd.Value)
                .ToListAsync();

            // Generate PDF
            var pdfService = new Services.AssessmentCompletePdfService();
            var pdfBytes = pdfService.GenerateCompletePdf(
                patient,
                socrate,
                assessment,
                tests,
                tintivData,
                clinicalData
            );

            var fileName = $"Bilan_Complet_{patient.LastName}_{patient.FirstName}_{assessment.Date:yyyyMMdd}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

    }
}