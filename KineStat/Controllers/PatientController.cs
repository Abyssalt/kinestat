using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using KineStat.Filters;
using KineStat.Helpers;

namespace KineStat.Controllers
{
    [AuthorizePhysio]
    public class PatientController : Controller
    {
        private readonly KineDbContext _context;
        public PatientController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles the HTTP GET request to display the anamnesis view for a specified patient.
        /// </summary>
        /// <remarks>The returned view includes the patient's folder information and a list of
        /// physiotherapists ordered by last name, available via <see cref="ViewBag.Physios"/>.</remarks>
        /// <param name="id">The unique identifier of the patient whose anamnesis information is to be displayed.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the anamnesis view for the patient if found; otherwise, a
        /// NotFound result if the patient does not exist.</returns>
        [Route("Patient/{id}/Anamnese")]
        public async Task<IActionResult> Anamnese(int id)
        {
            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, id))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            var patient = _context.Patients
                .Include(p => p.Folders)
                .Include(p => p.Doctor)
                .FirstOrDefault(p => p.Id == id);

            if (patient == null)
                return NotFound();

            ViewBag.Physios = await _context.Physios
                .OrderBy(p => p.LastName)
                .ToListAsync();

            ViewBag.Doctors = await _context.Doctors
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToListAsync();

            return View(patient);
        }

        /// <summary>
        /// Handles the submission of patient edits, including updating patient details and optionally adding a new
        /// doctor if provided.
        /// </summary>
        /// <remarks>All doctor fields must be provided to add a new doctor; partial input will result in
        /// a validation error. Only the physiotherapist who owns the patient can edit their details. If the patient's
        /// status is set to inactive, the inactivity date is updated accordingly. Concurrency and validation errors are
        /// handled and reported to the user.</remarks>
        /// <param name="patient">The patient entity containing the updated information. Must have a valid identifier and pass model
        /// validation.</param>
        /// <param name="NewDoctorLastName">The last name of the new doctor to associate with the patient. Required if adding a new doctor; otherwise,
        /// can be null or empty.</param>
        /// <param name="NewDoctorFirstName">The first name of the new doctor to associate with the patient. Required if adding a new doctor; otherwise,
        /// can be null or empty.</param>
        /// <param name="NewDoctorINAMI">The INAMI number of the new doctor to associate with the patient. Must be exactly 11 digits if provided.
        /// Required if adding a new doctor; otherwise, can be null or empty.</param>
        /// <returns>A redirect to the patient's details page if the update is successful, or to the appropriate page with an
        /// error message if validation fails or an error occurs.</returns>
        [HttpPost]
        [Route("Patient/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Patient patient, string? NewDoctorLastName, string? NewDoctorFirstName, string? NewDoctorINAMI)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (ModelState.IsValid)
                {
                    var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

                    if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, patient.Id))
                    {
                        TempData["Error"] = "Vous n'avez pas accès à ce patient.";
                        await transaction.RollbackAsync();
                        return RedirectToAction("Index", "Patients");
                    }

                    var existingPatient = await _context.Patients.FindAsync(patient.Id);
                    if (existingPatient == null)
                    {
                        TempData["Error"] = "Patient introuvable.";
                        await transaction.RollbackAsync();
                        return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
                    }

                    // ===== STATUS AND INACTIVITY MANAGEMENT (GDPR) =====
                    var oldStatus = existingPatient.Status;
                    var newStatus = patient.Status;

                    if (newStatus == PatientStatus.Inactif && oldStatus != PatientStatus.Inactif)
                    {
                        existingPatient.InactiveSinceDate = DateTime.UtcNow;
                    }
                    else if (oldStatus == PatientStatus.Inactif && newStatus != PatientStatus.Inactif)
                    {
                        existingPatient.InactiveSinceDate = null; // Réinitialiser le compteur
                    }
                    // ===== END STATUS AND INACTIVITY MANAGEMENT =====

                    if (!string.IsNullOrWhiteSpace(NewDoctorLastName) &&
                        !string.IsNullOrWhiteSpace(NewDoctorFirstName) &&
                        !string.IsNullOrWhiteSpace(NewDoctorINAMI))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(NewDoctorINAMI, @"^\d{11}$"))
                        {
                            TempData["Error"] = "Le numéro INAMI du médecin doit contenir exactement 11 chiffres.";
                            await transaction.RollbackAsync();
                            return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
                        }

                        var existingDoctor = await _context.Doctors
                            .FirstOrDefaultAsync(d => d.NumeroINAMI == NewDoctorINAMI);

                        if (existingDoctor != null)
                        {
                            TempData["Error"] = $"Un médecin avec ce numéro INAMI existe déjà : Dr. {existingDoctor.FirstName} {existingDoctor.LastName}";
                            await transaction.RollbackAsync();
                            return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
                        }

                        var newDoctor = new Doctor
                        {
                            LastName = NewDoctorLastName,
                            FirstName = NewDoctorFirstName,
                            NumeroINAMI = NewDoctorINAMI
                        };

                        _context.Doctors.Add(newDoctor);
                        await _context.SaveChangesAsync();

                        patient.DoctorId = newDoctor.Id;
                    }
                    else if (!string.IsNullOrWhiteSpace(NewDoctorLastName) ||
                             !string.IsNullOrWhiteSpace(NewDoctorFirstName) ||
                             !string.IsNullOrWhiteSpace(NewDoctorINAMI))
                    {
                        TempData["Error"] = "Si vous souhaitez ajouter un nouveau médecin, tous les champs (Nom, Prénom, INAMI) doivent être remplis.";
                        await transaction.RollbackAsync();
                        return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
                    }

                    existingPatient.FirstName = patient.FirstName;
                    existingPatient.LastName = patient.LastName;
                    existingPatient.Email = patient.Email;
                    existingPatient.PhoneNumber = patient.PhoneNumber;
                    existingPatient.Gender = patient.Gender;
                    existingPatient.Status = patient.Status;
                    existingPatient.BirthDate = patient.BirthDate;
                    existingPatient.SocialSecurityNumber = patient.SocialSecurityNumber;
                    existingPatient.PhysioId = patient.PhysioId;
                    existingPatient.Weight = patient.Weight;
                    existingPatient.Height = patient.Height;
                    existingPatient.DoctorId = patient.DoctorId;
                    existingPatient.Address = patient.Address;
                    existingPatient.Country = patient.Country;

                    _context.Update(existingPatient);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    TempData["Success"] = $"Patient {patient.FirstName} {patient.LastName} modifié avec succès.";
                    return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Erreur de validation : " + string.Join(", ", errors);
                await transaction.RollbackAsync();
                return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "Erreur de concurrence lors de la modification. Le patient a peut-être été modifié ou supprimé.";
                return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = $"Erreur lors de la modification du patient : {ex.Message}";
                return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
            }
        }

        /// <summary>
        /// Updates the medical information for a specified patient and saves the changes to the database.
        /// </summary>
        /// <remarks>The caller must have ownership of the patient to update their medical information. If
        /// the patient does not exist or the user does not have access, the method redirects with an error
        /// message.</remarks>
        /// <param name="PatientId">The unique identifier of the patient whose medical information is to be updated.</param>
        /// <param name="Profession">The patient's current profession. Can be null to clear the existing value.</param>
        /// <param name="ActivitesPhysiques">A description of the patient's physical activities. Can be null to clear the existing value.</param>
        /// <param name="AntecedentsMedicaux">The patient's medical history or antecedents. Can be null to clear the existing value.</param>
        /// <param name="MedicationActuelle">The patient's current medication. Can be null to clear the existing value.</param>
        /// <returns>A redirect to the patient's medical history page if the update is successful; otherwise, a redirect to an
        /// appropriate page with an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateMedicalInfo(int PatientId, string? Profession, string? ActivitesPhysiques, string? AntecedentsMedicaux, string? MedicationActuelle)
        {
            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, PatientId))
            {
                TempData["Error"] = "Vous n'avez pas accès à ce patient.";
                return RedirectToAction("Index", "Patients");
            }

            var patient = await _context.Patients.FindAsync(PatientId);

            if (patient == null)
            {
                TempData["Error"] = "Patient introuvable";
                return RedirectToAction("Anamnese", new { id = PatientId });
            }

            patient.Profession = Profession;
            patient.PhysicalActivities = ActivitesPhysiques;
            patient.MedicalHistory = AntecedentsMedicaux;
            patient.ActualMedication = MedicationActuelle;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Informations médicales mises à jour avec succès";
            return RedirectToAction("Anamnese", new { id = PatientId });
        }

        /// <summary>
        /// Handles the HTTP POST request to save the anamnese data for the specified patient and redirects to the
        /// anamnese view.
        /// </summary>
        /// <remarks>This action does not persist any data directly. It performs a redirect to the
        /// "Anamnese" view, passing the patient identifier as a route value.</remarks>
        /// <param name="id">The unique identifier of the patient whose anamnese data is being saved.</param>
        /// <returns>A redirect result to the anamnese view for the specified patient.</returns>
        [HttpPost]
        [Route("Patient/{id}/SaveAnamnese")]
        public IActionResult SaveAnamnese(int id)
        {
            return RedirectToAction("Anamnese", new { id = id });
        }
    }
}