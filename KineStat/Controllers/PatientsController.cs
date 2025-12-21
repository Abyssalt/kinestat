using KineStat.Data;
using KineStat.Models;
using KineStat.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KineStat.Filters;
using KineStat.Helpers;


namespace KineStat.Controllers
{
    [AuthorizePhysio]
    public class PatientsController : Controller
    {
        private readonly KineDbContext _context;

        public PatientsController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles HTTP GET requests to display a list of patients, optionally filtered by search term and status.
        /// Excludes patients with "Terminé" status by default (anonymized patients).
        /// </summary>
        /// <remarks>The returned view includes a list of patients ordered by last name, as well as a list
        /// of available physiotherapists for display. The search term and status filter are preserved in the view for
        /// user convenience.</remarks>
        /// <param name="search">An optional search term used to filter patients by first or last name. If null or whitespace, no name
        /// filtering is applied.</param>
        /// <param name="status">An optional status value used to filter patients by their current status. Must be a valid value of the
        /// PatientStatus enumeration; otherwise, no status filtering is applied.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult that renders
        /// the patient list view with the applied filters.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(string search, string status)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userRole != "Physio" || string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            int physioId = int.Parse(userIdString);

            var query = _context.Patients
                .Include(p => p.Physio)
                .Where(p => p.PhysioId == physioId)
                .AsQueryable();

            bool hasFilter = !string.IsNullOrWhiteSpace(status) && status != "Tous";

            if (!hasFilter || (hasFilter && status != "Terminé"))
            {
                query = query.Where(p => p.Status != PatientStatus.Terminé);
            }

            List<Patient> patients = new List<Patient>();

            bool hasSearch = !string.IsNullOrWhiteSpace(search);

            if (hasSearch || hasFilter)
            {
                if (hasSearch)
                {
                    var searchLower = search.ToLower();
                    query = query.Where(p =>
                        p.FirstName.ToLower().Contains(searchLower) ||
                        p.LastName.ToLower().Contains(searchLower)
                    );
                }

                if (hasFilter && Enum.TryParse<PatientStatus>(status, out var patientStatus))
                {
                    query = query.Where(p => p.Status == patientStatus);
                }

                patients = await query.OrderBy(p => p.LastName).ToListAsync();
            }

            ViewBag.Physios = await _context.Physios
                .OrderBy(p => p.LastName)
                .ToListAsync();

            ViewBag.Doctors = await _context.Doctors
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .ToListAsync();

            ViewBag.SearchTerm = search;
            ViewBag.StatusFilter = status;

            return View(patients);
        }

        /// <summary>
        /// Searches for patients matching the specified criteria and returns a paginated list of results.
        /// Excludes patients with "Terminé" status by default (anonymized patients).
        /// </summary>
        /// <remarks>The returned patient list is ordered by last name. If no patients match the criteria,
        /// the list will be empty. This method supports server-side paging for efficient data retrieval.</remarks>
        /// <param name="search">The search term used to filter patients by first or last name. If null or empty, no name filtering is
        /// applied.</param>
        /// <param name="status">The patient status to filter by. Must be a valid value of the PatientStatus enumeration, or "Tous" to
        /// include all statuses. If null or empty, no status filtering is applied.</param>
        /// <param name="page">The page number of results to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of patients to include per page. Must be greater than 0.</param>
        /// <returns>A JSON result containing the filtered list of patients, the total count of matching patients, the current
        /// page number, and the total number of pages.</returns>
        [HttpGet]
        public async Task<JsonResult> SearchPatients(string search, string status, int page = 1, int pageSize = 5)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userRole != "Physio" || string.IsNullOrEmpty(userIdString))
            {
                return Json(new
                {
                    patients = new List<object>(),
                    totalCount = 0,
                    currentPage = 1,
                    totalPages = 0
                });
            }

            int physioId = int.Parse(userIdString);

            var query = _context.Patients
                .Include(p => p.Physio)
                .Where(p => p.PhysioId == physioId)
                .AsQueryable();

            bool hasStatusFilter = !string.IsNullOrWhiteSpace(status) && status != "Tous";

            if (!hasStatusFilter || (hasStatusFilter && status != "Terminé"))
            {
                query = query.Where(p => p.Status != PatientStatus.Terminé);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(p =>
                    p.FirstName.ToLower().Contains(searchLower) ||
                    p.LastName.ToLower().Contains(searchLower)
                );
            }

            if (hasStatusFilter && Enum.TryParse<PatientStatus>(status, out var patientStatus))
            {
                query = query.Where(p => p.Status == patientStatus);
            }

            var totalCount = await query.CountAsync();

            var patients = await query
                .OrderBy(p => p.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.Email,
                    p.Gender,
                    p.BirthDate,
                    p.PhoneNumber,
                    p.Status
                })
                .ToListAsync();

            return Json(new
            {
                patients,
                totalCount,
                currentPage = page,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        /// <summary>
        /// Handles HTTP POST requests to create a new patient. Validates the input and ensures that the
        /// patient's social security number and phone number are unique before saving.
        /// </summary>
        /// <remarks>If the provided patient data is invalid or duplicates an existing patient's social
        /// security number or phone number, the method does not create a new record and displays an appropriate error
        /// message. The method uses anti-forgery validation to protect against cross-site request forgery (CSRF)
        /// attacks.</remarks>
        /// <param name="patient">The patient entity containing the details to be created. Must have a unique social security number and phone
        /// number. Cannot be null.</param>
        /// <returns>An IActionResult that redirects to the patient list view. If creation succeeds, a success message is
        /// displayed; otherwise, an error message is shown.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient, string? NewDoctorLastName, string? NewDoctorFirstName, string? NewDoctorINAMI)
        {

            var userRole = HttpContext.Session.GetString("UserRole");
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userRole != "Physio" || string.IsNullOrEmpty(userIdString))
            {
                TempData["Error"] = "Vous devez être connecté en tant que kinésithérapeute pour créer un patient.";
                return RedirectToAction("Login", "Account");
            }

            patient.PhysioId = int.Parse(userIdString);

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var existingPatientByNiss = await _context.Patients
                        .FirstOrDefaultAsync(p => p.SocialSecurityNumber == patient.SocialSecurityNumber);

                    if (existingPatientByNiss != null)
                    {
                        TempData["Error"] = $"Un patient avec ce numéro de sécurité sociale existe déjà : {existingPatientByNiss.FirstName} {existingPatientByNiss.LastName}";
                        await transaction.RollbackAsync();
                        return RedirectToAction(nameof(Index));
                    }

                    var existingPatientByPhone = await _context.Patients
                        .FirstOrDefaultAsync(p => p.PhoneNumber == patient.PhoneNumber);

                    if (existingPatientByPhone != null)
                    {
                        TempData["Error"] = $"Un patient avec ce numéro de téléphone existe déjà : {existingPatientByPhone.FirstName} {existingPatientByPhone.LastName}";
                        await transaction.RollbackAsync();
                        return RedirectToAction(nameof(Index));
                    }

                    if (!string.IsNullOrWhiteSpace(NewDoctorLastName) &&
                        !string.IsNullOrWhiteSpace(NewDoctorFirstName) &&
                        !string.IsNullOrWhiteSpace(NewDoctorINAMI))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(NewDoctorINAMI, @"^\d{11}$|^\d-\d{5}-\d{2}-\d{3}$"))
                        {
                            TempData["Error"] = "Le numéro INAMI du médecin doit contenir 11 chiffres.";
                            await transaction.RollbackAsync();
                            return RedirectToAction(nameof(Index));
                        }

                        var existingDoctor = await _context.Doctors
                            .FirstOrDefaultAsync(d => d.NumeroINAMI == NewDoctorINAMI);

                        if (existingDoctor != null)
                        {
                            TempData["Error"] = $"Un médecin avec ce numéro INAMI existe déjà : Dr. {existingDoctor.FirstName} {existingDoctor.LastName}";
                            await transaction.RollbackAsync();
                            return RedirectToAction(nameof(Index));
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
                        return RedirectToAction(nameof(Index));
                    }

                    patient.Status = PatientStatus.Actif;
                    _context.Add(patient);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return RedirectToAction("Anamnese","Patient", new { id = patient.Id });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
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

        /// <summary>
        /// Deletes the patient record with the specified identifier and redirects to the patient list view.
        /// </summary>
        /// <remarks>This action requires a valid anti-forgery token and is accessible via an HTTP POST
        /// request. If the patient does not exist, no deletion occurs and an error message is set. Success and error
        /// messages are communicated using TempData for display in the redirected view.</remarks>
        /// <param name="id">The unique identifier of the patient to delete.</param>
        /// <returns>A redirect to the patient list view. If the patient is not found or an error occurs during deletion, an
        /// error message is provided via TempData.</returns>
        [HttpPost]
        [Route("Index/{id}/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var physioId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (!await PatientOwnershipHelper.IsPatientOwnedByPhysio(_context, physioId, id))
            {
                TempData["Error"] = "Vous n'avez pas accès à ce patient.";
                return RedirectToAction(nameof(Index));
            }

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
}