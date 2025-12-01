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
            Console.WriteLine($"=== DEBUG Tests Action ===");
            Console.WriteLine($"Patient ID: {id}");

            // Récupère le patient
            var patient = await _context.Patients
                .Include(p => p.Physio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                Console.WriteLine("ERROR: Patient NOT FOUND");
                TempData["Error"] = "Patient introuvable.";
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine($"Patient found: {patient.FirstName} {patient.LastName}");

            // ✅ Récupère TOUS les clusters avec leurs questions
            var clusters = await _context.Cluster
                .Include(c => c.Questions)
                .ToListAsync();

            Console.WriteLine($"Clusters found: {clusters.Count}");

            if (clusters.Count == 0)
            {
                Console.WriteLine("WARNING: No clusters in database!");
                TempData["Warning"] = "Aucun cluster de tests disponible.";
                return RedirectToAction("Index", new { id });
            }

            // Prépare les données pour la vue
            ViewData["Patient"] = patient;
            ViewData["PatientId"] = id;
            ViewData["PatientNom"] = $"{patient.FirstName} {patient.LastName}";
            ViewData["Clusters"] = clusters;
            ViewData["Breadcrumbs"] = $"<li class='breadcrumb-item'><a href='/'>Accueil</a></li>" +
                                      $"<li class='breadcrumb-item'><a href='/Patients'>Patients</a></li>" +
                                      $"<li class='breadcrumb-item'><a href='/Patients/Anamnese/{id}'>Anamnèse</a></li>" +
                                      $"<li class='breadcrumb-item active' aria-current='page'>Tests</li>";

            Console.WriteLine($"Returning view with {clusters.Count} clusters");

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

                // Sauvegarde des réponses
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
                        // Test personnalisé
                        response.CustomTestName = test.Name;
                        response.CustomTestType = test.Type;
                        response.QuestionId = null;
                        response.AnswerId = null;

                        Console.WriteLine($"Test personnalisé: {test.Name} = {test.Value}");
                    }
                    else
                    {
                        // Test standard
                        response.QuestionId = test.Id;

                        // ✅ CORRECTION : Vérifie d'abord le type de question
                        var question = await _context.Questions.FindAsync(test.Id);

                        if (question is QuestionQCM)
                        {
                            // ✅ Charge le QuestionQCM avec ses Answers
                            var qcm = await _context.QuestionQCMs
                                .Include(q => q.Answers)
                                .FirstOrDefaultAsync(q => q.Id == test.Id);

                            if (qcm != null)
                            {
                                // Cherche l'Answer qui correspond au titre sélectionné
                                var answer = qcm.Answers?.FirstOrDefault(a => a.Title == test.Value);
                                if (answer != null)
                                {
                                    response.AnswerId = answer.Id;
                                    Console.WriteLine($"QuestionQCM #{test.Id}: Answer '{answer.Title}' (ID: {answer.Id})");
                                }
                                else
                                {
                                    Console.WriteLine($"⚠️ Answer non trouvé pour la valeur '{test.Value}'");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Question #{test.Id} (type: {question?.GetType().Name}) = {test.Value}");
                        }
                    }

                    _context.PatientAnswerTests.Add(response);
                    savedCount++;
                }

                await _context.SaveChangesAsync();

                Console.WriteLine($"✓ {savedCount} réponses enregistrées");

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
                Console.WriteLine($"❌ Erreur SaveTestResults: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

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