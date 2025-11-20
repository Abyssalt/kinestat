using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace KineStat.Controllers
{
    public class PatientsController : Controller
    {
        private readonly KineDbContext _context;

        public PatientsController(KineDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var patients = await _context.Patients
                .Include(p => p.Physio)
                .ToListAsync();

            ViewBag.Physios = await _context.Physio
                .OrderBy(p => p.LastName)
                .ToListAsync();

            return View(patients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Patient {patient.FirstName} {patient.LastName} créé avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erreur lors de la création : {ex.Message}";
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner: " + ex.InnerException.Message);

                        if (ex.InnerException.InnerException != null)
                        {
                            Console.WriteLine("Inner Inner: " + ex.InnerException.InnerException.Message);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["Error"] = "Données invalides. Veuillez vérifier le formulaire.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AutoSavePatient([FromBody] PatientAutoSaveRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.LastName) ||
                    string.IsNullOrWhiteSpace(request.FirstName) ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.PhoneNumber) ||
                    request.BirthDate == default ||
                    request.SocialSecurityNumber == 0 ||
                    request.PhysioId == 0)
                {
                    return Json(new { success = false, message = "Champs obligatoires manquants", isComplete = false });
                }

                var patient = new Patient
                {
                    LastName = request.LastName,
                    FirstName = request.FirstName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Genre = request.Genre,
                    BirthDate = request.BirthDate,
                    SocialSecurityNumber = request.SocialSecurityNumber,
                    PhysioId = request.PhysioId,
                    Weight = request.Weight,
                    Height = request.Height,
                    DoctorName = request.DoctorName,
                    DoctorINAMI = request.DoctorINAMI
                };

                _context.Add(patient);
                await _context.SaveChangesAsync();

                return Json(new { success = true, patientId = patient.Id, message = "Patient créé avec succès", isComplete = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erreur: {ex.Message}", isComplete = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AutoSaveField([FromBody] AutoSaveFieldRequest request)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(request.PatientId);

                if (patient == null)
                {
                    return Json(new { success = false, message = "Patient introuvable" });
                }

                SetPatientField(patient, request.FieldName, request.FieldValue);

                _context.Update(patient);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Champ sauvegardé" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erreur: {ex.Message}" });
            }
        }

        private void SetPatientField(Patient patient, string fieldName, string fieldValue)
        {
            switch (fieldName)
            {
                case "LastName":
                    patient.LastName = fieldValue;
                    break;
                case "FirstName":
                    patient.FirstName = fieldValue;
                    break;
                case "Email":
                    patient.Email = fieldValue;
                    break;
                case "PhoneNumber":
                    patient.PhoneNumber = fieldValue;
                    break;
                case "Address":
                    patient.Address = fieldValue;
                    break;
                case "Genre":
                    patient.Genre = (Gender)int.Parse(fieldValue);
                    break;
                case "BirthDate":
                    patient.BirthDate = DateTime.Parse(fieldValue);
                    break;
                case "SocialSecurityNumber":
                    patient.SocialSecurityNumber = int.Parse(fieldValue);
                    break;
                case "PhysioId":
                    patient.PhysioId = int.Parse(fieldValue);
                    break;
                case "Weight":
                    patient.Weight = string.IsNullOrWhiteSpace(fieldValue) ? 0 : double.Parse(fieldValue);
                    break;
                case "Height":
                    patient.Height = string.IsNullOrWhiteSpace(fieldValue) ? 0 : double.Parse(fieldValue);
                    break;
                case "DoctorName":
                    patient.DoctorName = fieldValue;
                    break;
                case "DoctorINAMI":
                    patient.DoctorINAMI = fieldValue;
                    break;
            }
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
                        return RedirectToAction(nameof(Index));
                    }

                    existingPatient.FirstName = patient.FirstName;
                    existingPatient.LastName = patient.LastName;
                    existingPatient.Email = patient.Email;
                    existingPatient.PhoneNumber = patient.PhoneNumber;
                    existingPatient.Genre = patient.Genre;
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
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Données invalides. Veuillez vérifier les champs.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Error"] = "Erreur de concurrence lors de la modification. Le patient a peut-être été modifié ou supprimé.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erreur lors de la modification du patient.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                TempData["Error"] = "Patient introuvable.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Patient {patient.FirstName} {patient.LastName} supprimé avec succès.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de la suppression : {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }

    public class PatientAutoSaveRequest
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Gender Genre { get; set; }
        public DateTime BirthDate { get; set; }
        public int SocialSecurityNumber { get; set; }
        public int PhysioId { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string DoctorName { get; set; }
        public string DoctorINAMI { get; set; }
    }

    public class AutoSaveFieldRequest
    {
        public int PatientId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}