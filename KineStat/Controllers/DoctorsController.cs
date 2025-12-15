using KineStat.Data;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KineStat.Filters;

namespace KineStat.Controllers
{
    [AuthorizePhysio]
    public class DoctorsController : Controller
    {
        private readonly KineDbContext _context;

        public DoctorsController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieve all the doctors for display in a dropdown list
        /// </summary>
        /// <returns>List of doctors in JSON format</returns>
        [HttpGet]
        public async Task<JsonResult> GetAllDoctors()
        {
            var doctors = await _context.Doctors
                .OrderBy(d => d.LastName)
                .ThenBy(d => d.FirstName)
                .Select(d => new
                {
                    d.Id,
                    d.FirstName,
                    d.LastName,
                    d.NumeroINAMI,
                    FullName = d.FirstName + " " + d.LastName
                })
                .ToListAsync();

            return Json(doctors);
        }

        /// <summary>
        /// Create a new doctor via an AJAX request
        /// </summary>
        /// <param name="doctor">The doctor’s data to be created</param>
        /// <returns>The doctor created or an error</returns>
        [HttpPost]
        public async Task<JsonResult> CreateDoctor([FromBody] Doctor doctor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                var existingDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.NumeroINAMI == doctor.NumeroINAMI);

                if (existingDoctor != null)
                {
                    return Json(new 
                    { 
                        success = false, 
                        message = $"Un médecin avec le numéro INAMI {doctor.NumeroINAMI} existe déjà : Dr. {existingDoctor.FirstName} {existingDoctor.LastName}" 
                    });
                }

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                return Json(new 
                { 
                    success = true, 
                    message = $"Médecin Dr. {doctor.FirstName} {doctor.LastName} créé avec succès",
                    doctor = new
                    {
                        doctor.Id,
                        doctor.FirstName,
                        doctor.LastName,
                        doctor.NumeroINAMI,
                        FullName = $"{doctor.FirstName} {doctor.LastName}"
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new 
                { 
                    success = false, 
                    message = $"Erreur lors de la création du médecin : {ex.Message}" 
                });
            }
        }

        /// <summary>
        /// Search for a doctor by INAMI number
        /// </summary>
        /// <param name="inami">The INAMI number to look for</param>
        /// <returns>The doctor found or null</returns>
        [HttpGet]
        public async Task<JsonResult> SearchByINAMI(string inami)
        {
            if (string.IsNullOrWhiteSpace(inami))
            {
                return Json(new { success = false, message = "Numéro INAMI requis" });
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.NumeroINAMI == inami);

            if (doctor == null)
            {
                return Json(new { success = false, message = "Aucun médecin trouvé avec ce numéro INAMI" });
            }

            return Json(new 
            { 
                success = true,
                doctor = new
                {
                    doctor.Id,
                    doctor.FirstName,
                    doctor.LastName,
                    doctor.NumeroINAMI,
                    FullName = $"{doctor.FirstName} {doctor.LastName}"
                }
            });
        }
    }
}
