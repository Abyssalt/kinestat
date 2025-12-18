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
        /// Displays the login page for users who are not currently authenticated, or redirects authenticated users to
        /// their respective dashboard based on their role.
        /// </summary>
        /// <remarks>If the user is already logged in, this method redirects to either the Patients or
        /// Admin dashboard depending on the user's role stored in the session. Otherwise, it clears any temporary data
        /// and displays the login view.</remarks>
        /// <returns>An <see cref="IActionResult"/> that renders the login view for unauthenticated users, or redirects
        /// authenticated users to the appropriate dashboard.</returns>
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
        /// Authenticates a user based on the provided login credentials and initiates a session for the authenticated
        /// user.
        /// </summary>
        /// <remarks>If authentication is successful, the user's session is initialized and a success
        /// message is displayed. If authentication fails or an error occurs, an error message is added to the model
        /// state and the login view is returned. This action requires a valid anti-forgery token and is intended for
        /// HTTP POST requests.</remarks>
        /// <param name="model">The login information submitted by the user, including email and password. Must not be null and must contain
        /// valid credentials.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the login view if authentication fails or redirects to the
        /// appropriate dashboard upon successful login.</returns>
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
        /// Logs out the current user by clearing the session and temporary data, then redirects to the login page.
        /// </summary>
        /// <remarks>This method removes all session and temporary data associated with the current user.
        /// After logout, the user must authenticate again to access protected resources.</remarks>
        /// <returns>A <see cref="IActionResult"/> that redirects the user to the login page.</returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData.Clear();
            TempData["InfoMessage"] = "Déconnecté avec succès.";
            return RedirectToAction("Login");
        }
    }
}