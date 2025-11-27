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
                        return RedirectToAction(nameof(Anamnese));
                    }

                    existingPatient.FirstName = patient.FirstName;
                    existingPatient.LastName = patient.LastName;
                    existingPatient.Email = patient.Email;
                    existingPatient.PhoneNumber = patient.PhoneNumber;
                    existingPatient.Gender = patient.Gender;
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
                    return RedirectToAction(nameof(Anamnese));
                }
                return RedirectToAction(nameof(Anamnese));
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Error"] = "Erreur de concurrence lors de la modification. Le patient a peut-être été modifié ou supprimé.";
                return RedirectToAction(nameof(Anamnese));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erreur lors de la modification du patient.";
                return RedirectToAction(nameof(Anamnese));
            }
        }


        [HttpPost]
        public IActionResult SaveAnamnese()
        {
            return RedirectToAction("Anamnese", new { id = 1 });
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