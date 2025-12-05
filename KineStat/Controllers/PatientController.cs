using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KineStat.Controllers
{
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
        /// <remarks>The returned view includes the patient's dossier information and a list of
        /// physiotherapists ordered by last name, available via <see cref="ViewBag.Physios"/>.</remarks>
        /// <param name="id">The unique identifier of the patient whose anamnesis information is to be displayed.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the anamnesis view for the patient if found; otherwise, a
        /// NotFound result if the patient does not exist.</returns>
        [Route("Patient/{id}/Anamnese")]
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

        /// <summary>
        /// Processes a POST request to update an existing patient's information. Redirects to the patient's anamnesis
        /// view upon completion.
        /// </summary>
        /// <remarks>This action requires a valid anti-forgery token and is accessible via the
        /// 'Patient/Edit' route. If the patient does not exist or validation fails, the user is redirected with an
        /// appropriate error message. Concurrency and other exceptions are handled and reported through
        /// TempData.</remarks>
        /// <param name="patient">The patient entity containing updated information. The patient's Id must correspond to an existing record.
        /// All required fields must be valid; otherwise, the update will not be performed.</param>
        /// <returns>An <see cref="IActionResult"/> that redirects to the anamnesis view for the patient. If the update is
        /// successful, a success message is provided; otherwise, an error message is displayed.</returns>
        [HttpPost]
        [Route("Patient/Edit")]
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
                    existingPatient.DoctorName = patient.DoctorName;
                    existingPatient.DoctorINAMI = patient.DoctorINAMI;
                    existingPatient.Address = patient.Address;

                    _context.Update(existingPatient);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Patient {patient.FirstName} {patient.LastName} modifié avec succès.";
                    return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Erreur de validation : " + string.Join(", ", errors);
                return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Error"] = "Erreur de concurrence lors de la modification. Le patient a peut-être été modifié ou supprimé.";
                return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de la modification du patient : {ex.Message}";
                return RedirectToAction(nameof(Anamnese), new { id = patient.Id });
            }
        }

        /// <summary>
        /// Updates the medical information for the specified patient and redirects to the patient's anamnesis view.
        /// </summary>
        /// <remarks>This action sets a success or error message in <see cref="TempData"/> depending on
        /// whether the update was successful. The method requires a valid patient identifier; if the patient does not
        /// exist, no changes are made.</remarks>
        /// <param name="PatientId">The unique identifier of the patient whose medical information is to be updated.</param>
        /// <param name="Profession">The patient's current profession. Can be null to leave unchanged.</param>
        /// <param name="ActivitesPhysiques">A description of the patient's physical activities. Can be null to leave unchanged.</param>
        /// <param name="AntecedentsMedicaux">A summary of the patient's medical history or antecedents. Can be null to leave unchanged.</param>
        /// <param name="MedicationActuelle">Details of the patient's current medication. Can be null to leave unchanged.</param>
        /// <returns>An <see cref="IActionResult"/> that redirects to the patient's anamnesis view. If the patient is not found,
        /// redirects with an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateMedicalInfo(int PatientId, string? Profession, string? ActivitesPhysiques, string? AntecedentsMedicaux, string? MedicationActuelle)
        {
            var patient = await _context.Patients.FindAsync(PatientId);

            if (patient == null)
            {
                TempData["Error"] = "Patient introuvable";
                return RedirectToAction("Anamnese", new { id = PatientId });
            }

            patient.Profession = Profession;
            patient.ActivitesPhysiques = ActivitesPhysiques;
            patient.AntecedentsMedicaux = AntecedentsMedicaux;
            patient.MedicationActuelle = MedicationActuelle;

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