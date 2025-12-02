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
                    var existingPatientByNiss = await _context.Patients
                        .FirstOrDefaultAsync(p => p.SocialSecurityNumber == patient.SocialSecurityNumber);

                    if (existingPatientByNiss != null)
                    {
                        TempData["Error"] = $"Un patient avec ce numéro de sécurité sociale existe déjà : {existingPatientByNiss.FirstName} {existingPatientByNiss.LastName}";
                        return RedirectToAction(nameof(Index));
                    }

                    var existingPatientByPhone = await _context.Patients
                        .FirstOrDefaultAsync(p => p.PhoneNumber == patient.PhoneNumber);

                    if (existingPatientByPhone != null)
                    {
                        TempData["Error"] = $"Un patient avec ce numéro de téléphone existe déjà : {existingPatientByPhone.FirstName} {existingPatientByPhone.LastName}";
                        return RedirectToAction(nameof(Index));
                    }

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
        [Route("Patients/{id}/Delete")]
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
                var today = dateResponse.Date;

                var existingResponses = await _context.PatientAnswerTests
                    .Where(pr => pr.PatientId == dto.PatientId &&
                                 pr.DateResponse.Date == today)
                    .ToListAsync();

                int savedCount = 0;
                int updatedCount = 0;

                foreach (var test in dto.Tests)
                {
                    PatientAnswerTests response;

                    if (test.Custom)
                    {
                        response = existingResponses.FirstOrDefault(r =>
                            r.IsCustomTest &&
                            r.CustomTestName == test.Name);

                        if (response != null)
                        {
                            response.ResponseValue = test.Value;
                            response.Observations = test.Observations;
                            response.DateResponse = dateResponse;
                            _context.PatientAnswerTests.Update(response);
                            updatedCount++;
                        }
                        else
                        {
                            response = new PatientAnswerTests
                            {
                                PatientId = dto.PatientId,
                                DateResponse = dateResponse,
                                ResponseValue = test.Value,
                                Observations = test.Observations,
                                IsCustomTest = true,
                                CustomTestName = test.Name,
                                CustomTestType = test.Type,
                                QuestionId = null,
                                AnswerId = null
                            };
                            _context.PatientAnswerTests.Add(response);
                            savedCount++;
                        }
                    }
                    else
                    {
                        response = existingResponses.FirstOrDefault(r =>
                            !r.IsCustomTest &&
                            r.QuestionId == test.Id);

                        if (response != null)
                        {
                            response.ResponseValue = test.Value;
                            response.Observations = test.Observations;
                            response.DateResponse = dateResponse;

                            var qcm = await _context.QuestionQCMs
                                .Include(q => q.Answers)
                                .FirstOrDefaultAsync(q => q.Id == test.Id);

                            if (qcm != null)
                            {
                                var answer = qcm.Answers?.FirstOrDefault(a => a.Title == test.Value);
                                response.AnswerId = answer?.Id;
                            }
                            else
                            {
                                response.AnswerId = null;
                            }

                            _context.PatientAnswerTests.Update(response);
                            updatedCount++;
                        }
                        else
                        {
                            response = new PatientAnswerTests
                            {
                                PatientId = dto.PatientId,
                                DateResponse = dateResponse,
                                QuestionId = test.Id,
                                ResponseValue = test.Value,
                                Observations = test.Observations,
                                IsCustomTest = false
                            };

                            var qcm = await _context.QuestionQCMs
                                .Include(q => q.Answers)
                                .FirstOrDefaultAsync(q => q.Id == test.Id);

                            if (qcm != null)
                            {
                                var answer = qcm.Answers?.FirstOrDefault(a => a.Title == test.Value);
                                response.AnswerId = answer?.Id;
                            }
                           
                            _context.PatientAnswerTests.Add(response);
                            savedCount++;
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Évaluation enregistrée avec succès",
                    testCount = savedCount + updatedCount,
                    newCount = savedCount,
                    updatedCount = updatedCount,
                    dateEvaluation = dateResponse.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
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

        [HttpGet]
        public async Task<IActionResult> GetQuestionsClinique(int id)
        {
            var allQuestions = await _context.Questions
                .Include(q => q.Category)
                .Where(q => q.ClusterId == null) 
                .OrderBy(q => q.CategoryId)
                .ThenBy(q => q.Title)
                .ToListAsync();

            var groupedQuestions = new Dictionary<string, List<object>>();

            foreach (var question in allQuestions)
            {
                var categoryName = question.Category?.Name ?? "Autre";

                if (!groupedQuestions.ContainsKey(categoryName))
                {
                    groupedQuestions[categoryName] = new List<object>();
                }

                object questionData;

                if (question is QuestionBool qBool)
                {
                    questionData = new
                    {
                        id = question.Id,
                        question = question.Title,
                        type = "bool",
                        options = new[] { "Oui", "Non" }
                    };
                }
                else if (question is QuestionQCM qQcm)
                {

                    var qcmWithAnswers = await _context.QuestionQCMs
                        .Include(q => q.Answers)
                        .FirstOrDefaultAsync(q => q.Id == question.Id);

                    questionData = new
                    {
                        id = question.Id,
                        question = question.Title,
                        type = "qcm",
                        options = qcmWithAnswers?.Answers?.Select(a => a.Title).ToArray() ?? Array.Empty<string>()
                    };
                }
                else if (question is QuestionLadder qLadder)
                {
                    questionData = new
                    {
                        id = question.Id,
                        question = question.Title,
                        type = "ladder",
                        options = Enumerable.Range(0, 11).Select(i => i.ToString()).ToArray() 
                    };
                }
                else
                {
                    questionData = new
                    {
                        id = question.Id,
                        question = question.Title,
                        type = "text",
                        options = Array.Empty<string>()
                    };
                }

                groupedQuestions[categoryName].Add(questionData);
            }

            return Json(groupedQuestions);
        }


        [HttpGet]
        public async Task<IActionResult> ExamenClinique(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Physio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            ViewData["Patient"] = patient;
            ViewData["PatientId"] = id;

            return View("~/Views/Patient/ExamenClinique.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> SaveExamenClinique([FromBody] SaveExamenCliniqueDTO dto)
        {
            try
            {
                var dateResponse = DateTime.UtcNow;
                var today = dateResponse.Date;

                var existingResponses = await _context.PatientAnswerTests
                    .Where(pr => pr.PatientId == dto.PatientId
                              && pr.DateResponse.Date == today
                              && !pr.IsCustomTest)
                    .ToListAsync();

                int savedCount = 0;
                int updatedCount = 0;

                foreach (var responseDto in dto.Responses)
                {
                    var question = await _context.Questions
                        .FirstOrDefaultAsync(q => q.Id == responseDto.QuestionId && q.ClusterId == null);

                    if (question == null)
                    {
                        continue; 
                    }

                    var existingResponse = existingResponses
                        .FirstOrDefault(r => r.QuestionId == responseDto.QuestionId);

                    if (existingResponse != null)
                    {
                        existingResponse.ResponseValue = responseDto.Response;
                        existingResponse.Observations = responseDto.Notes;
                        existingResponse.DateResponse = dateResponse;

                        _context.PatientAnswerTests.Update(existingResponse);
                        updatedCount++;
                    }
                    else
                    {
                        var newResponse = new PatientAnswerTests()
                        {
                            PatientId = dto.PatientId,
                            QuestionId = responseDto.QuestionId,
                            ResponseValue = responseDto.Response,
                            Observations = responseDto.Notes,
                            DateResponse = dateResponse,
                            IsCustomTest = false,
                            AnswerId = null
                        };

                        _context.PatientAnswerTests.Add(newResponse);
                        savedCount++;
                    }
                }

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Réponses enregistrées avec succès",
                    savedCount = savedCount,
                    updatedCount = updatedCount,
                    totalCount = savedCount + updatedCount
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Erreur lors de l'enregistrement: {ex.Message}"
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetExistingResponses(int id)
        {
            try
            {
                var today = DateTime.UtcNow.Date;

                var existingResponses = await _context.PatientAnswerTests
                    .Include(pr => pr.Question)
                    .ThenInclude(q => q.Category)
                    .Where(pr => pr.PatientId == id
                                 && pr.DateResponse.Date == today
                                 && !pr.IsCustomTest
                                 && pr.Question.ClusterId == null) 
                    .OrderByDescending(pr => pr.DateResponse)
                    .ToListAsync();


                var formattedResponses = existingResponses.Select(pr => new
                {
                    questionId = pr.QuestionId,
                    responseValue = pr.ResponseValue,
                    observations = pr.Observations,
                    categoryName = pr.Question?.Category?.Name ?? "Autre"
                }).ToList();

                return Json(formattedResponses);
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }

    }
}