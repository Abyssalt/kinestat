using Microsoft.AspNetCore.Mvc;
using KineStat.Data;
using KineStat.Models;
using KineStat.Models.ViewModels;
using KineStat.Services;
using Microsoft.EntityFrameworkCore;

namespace KineStat.Controllers
{
    public class AccountController : Controller
    {
        private readonly KineDbContext _context;

        public AccountController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Display login page (GET)
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            // If user already logged in, redirect to appropriate page
            if (HttpContext.Session.GetString("UserId") != null)
            {
                string userRole = HttpContext.Session.GetString("UserRole");
                if (userRole == "Physio")
                    return RedirectToAction("Index", "Patients");
                else if (userRole == "Admin")
                    return RedirectToAction("Index", "Admin");
            }

            TempData.Clear();

            return View();
        }

        /// <summary>
        /// Process the login form (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Check if form validation is passed
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var physio = await _context.Physios
                    .FirstOrDefaultAsync(p => p.Email == model.Email);

                if (physio != null && PasswordHasher.VerifyPassword(model.Password, physio.Password))
                {
                    HttpContext.Session.SetString("UserId", physio.Id.ToString());
                    HttpContext.Session.SetString("UserRole", "Physio");
                    HttpContext.Session.SetString("UserName", $"{physio.FirstName} {physio.LastName}");
                    HttpContext.Session.SetString("UserEmail", physio.Email);

                    TempData["SuccessMessage"] = $"Bienvenue, {physio.FirstName} !";
                    return RedirectToAction("Index", "Patients");
                }

                var admin = await _context.Administrators
                    .FirstOrDefaultAsync(a => a.Email == model.Email);

                if (admin != null && PasswordHasher.VerifyPassword(model.Password, admin.Password))
                {
                    HttpContext.Session.SetString("UserId", admin.Id.ToString());
                    HttpContext.Session.SetString("UserRole", "Admin");
                    HttpContext.Session.SetString("UserName", $"{admin.FirstName} {admin.LastName}");
                    HttpContext.Session.SetString("UserEmail", admin.Email);

                    TempData["SuccessMessage"] = $"Bienvenue, {admin.FirstName} !";
                    return RedirectToAction("Index", "Admin");
                }

                ModelState.AddModelError(string.Empty, "Email ou mot de passe incorrect.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la connexion.");
                // TODO: Add logging here
                return View(model);
            }
        }

        /// <summary>
        /// Logout user and clear the session
        /// </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData.Clear();
            TempData["InfoMessage"] = "Déconnecté avec succès.";
            return RedirectToAction("Login");
        }
    }
}