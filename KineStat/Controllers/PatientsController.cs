using KineStat.Data;
using KineStat.Models;
using KineStat.Models.DTO;
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
        public async Task<IActionResult> Index(string search, string status)
        {
            var query = _context.Patients.Include(p => p.Physio).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(p =>
                    p.FirstName.ToLower().Contains(searchLower) ||
                    p.LastName.ToLower().Contains(searchLower)
                );
            }

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<PatientStatus>(status, out var patientStatus))
            {
                query = query.Where(p => p.Status == patientStatus);
            }

            var patients = await query.OrderBy(p => p.LastName).ToListAsync();

            ViewBag.Physios = await _context.Physios
                .OrderBy(p => p.LastName)
                .ToListAsync();

            ViewBag.SearchTerm = search;
            ViewBag.StatusFilter = status;

            return View(patients);
        }

        [HttpGet]
        public async Task<JsonResult> SearchPatients(string search, string status, int page = 1, int pageSize = 5)
        {
            var query = _context.Patients.Include(p => p.Physio).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(p =>
                    p.FirstName.ToLower().Contains(searchLower) ||
                    p.LastName.ToLower().Contains(searchLower)
                );
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "Tous" && Enum.TryParse<PatientStatus>(status, out var patientStatus))
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    patient.Status = PatientStatus.Actif;
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



        [HttpGet]
        public async Task<IActionResult> Tests(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Physio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                TempData["Error"] = "Patient introuvable.";
                return RedirectToAction(nameof(Index));
            }


            var clusters = await _context.Cluster
                .Include(c => c.Questions)
                .ToListAsync();


            var today = DateTime.UtcNow.Date;
            var latestResponses = await _context.PatientAnswerTests
                .Where(pr => pr.PatientId == id)
                .OrderByDescending(pr => pr.DateResponse)
                .Take(100)  
                .ToListAsync();


            var latestSessionDate = latestResponses.FirstOrDefault()?.DateResponse.Date;
            if (latestSessionDate.HasValue)
            {
                latestResponses = latestResponses
                    .Where(pr => pr.DateResponse.Date == latestSessionDate.Value)
                    .ToList();

            }

            ViewData["Patient"] = patient;
            ViewData["PatientId"] = id;
            ViewData["PatientNom"] = $"{patient.FirstName} {patient.LastName}";
            ViewData["Clusters"] = clusters;
            ViewData["ExistingResponses"] = latestResponses;  
            ViewData["Breadcrumbs"] = $"<li class='breadcrumb-item'><a href='/'>Accueil</a></li>" +
                                      $"<li class='breadcrumb-item'><a href='/Patients'>Patients</a></li>" +
                                      $"<li class='breadcrumb-item'><a href='/Patients/Details/{id}'>{patient.FirstName} {patient.LastName}</a></li>" +
                                      $"<li class='breadcrumb-item active' aria-current='page'>Tests</li>";


            return View("~/Views/Patient/Tests.cshtml", clusters);
        }
        [HttpPost]
        public async Task<IActionResult> SaveTestResults([FromBody] SaveTestResultsDTO dto)
        {
            try
            {

                if (dto.PatientId <= 0)
                {
                    return BadRequest(new { success = false, message = "Patient invalide" });
                }

                var patient = await _context.Patients.FindAsync(dto.PatientId);
                if (patient == null)
                {
                    return NotFound(new { success = false, message = "Patient introuvable" });
                }

                var dateResponse = DateTime.UtcNow;
                int savedCount = 0;

                foreach (var test in dto.Tests)
                {
                    var response = new PatientAnswerTests()
                    {
                        PatientId = dto.PatientId,
                        DateResponse = dateResponse,
                        ResponseValue = test.Value,
                        Observations = test.Observations,
                        IsCustomTest = test.Custom
                    };

                    if (test.Custom)
                    {
                       
                        response.CustomTestName = test.Name;
                        response.CustomTestType = test.Type;
                        response.QuestionId = null;
                        response.AnswerId = null;
                    }
                    else
                    {

                        response.QuestionId = test.Id;

                        var question = await _context.Questions.FindAsync(test.Id);

                        if (question is QuestionQCM)
                        {
                            var qcm = await _context.QuestionQCMs
                                .Include(q => q.Answers)
                                .FirstOrDefaultAsync(q => q.Id == test.Id);

                            if (qcm != null)
                            {
                                var answer = qcm.Answers?.FirstOrDefault(a => a.Title == test.Value);
                                if (answer != null)
                                {
                                    response.AnswerId = answer.Id;
                                   
                                }
                               
                            }
                        }
                       
                    }

                    _context.PatientAnswerTests.Add(response);
                    savedCount++;
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Évaluation enregistrée avec succès",
                    testCount = savedCount,
                    dateEvaluation = dateResponse.ToString("dd/MM/yyyy HH:mm")
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    success = false,
                    message = "Erreur lors de l'enregistrement",
                    error = ex.Message
                });
            }
        }
    }
}