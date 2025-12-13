using KineStat.Data;
using KineStat.Models;
using KineStat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KineStat.Controllers
{
    public class AdminController : Controller
    {
        private readonly KineDbContext _context;

        public AdminController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles HTTP GET requests and returns a view displaying a list of physiotherapists, including their
        /// associated patients, ordered by last name.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> that renders the view with the list of physiotherapists and their patients.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var physios = await _context.Physios
                .Include(p => p.Patients)
                .OrderBy(p => p.LastName)
                .ToListAsync();

            return View(physios);
        }

        /// <summary>
        /// Handles HTTP POST requests to create a new physio record using the provided data.
        /// </summary>
        /// <remarks>This action requires a valid anti-forgery token and expects model validation to be
        /// performed prior to creation. Success and error messages are stored in TempData for display after
        /// redirection.</remarks>
        /// <param name="physio">The physio entity containing the details to be created. Must have valid model state; required fields should
        /// be populated.</param>
        /// <param name="passwordConfirm">The password confirmation field to verify password entry.</param>
        /// <returns>A redirect to the index view upon successful creation or if an error occurs. If the model state is invalid,
        /// redirects to the index view with an error message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Physio physio, string passwordConfirm)
        {
            // Vérifier la force du mot de passe
            if (!IsPasswordStrong(physio.Password, out List<string> errors))
            {
                TempData["Error"] = "Mot de passe faible : " + string.Join(", ", errors);
                return RedirectToAction(nameof(Index));
            }

            if (physio.Password != passwordConfirm)
            {
                TempData["Error"] = "Les mots de passe ne correspondent pas.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    physio.Password = PasswordHasher.HashPassword(physio.Password);
                    _context.Physios.Add(physio);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Kinésithérapeute {physio.FirstName} {physio.LastName} créé avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erreur lors de la création : {ex.Message}";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["Error"] = "Données invalides. Veuillez vérifier le formulaire.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Processes a POST request to update the details of an existing physiotherapist.
        /// </summary>
        /// <remarks>This action requires a valid anti-forgery token and model state. If the
        /// physiotherapist does not exist or the model is invalid, the user is redirected with an error message.
        /// Handles concurrency and general exceptions by displaying appropriate error messages.</remarks>
        /// <param name="physio">The physiotherapist entity containing the updated information. Must have a valid identifier and pass model
        /// validation.</param>
        /// <param name="passwordConfirm">The password confirmation field to verify password entry when changing password.</param>
        /// <returns>A redirect to the index view. If the update is successful, a success message is displayed; otherwise, an
        /// error message is shown.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Physio physio, string passwordConfirm)
        {
            try
            {
                if (!string.IsNullOrEmpty(physio.Password) || !string.IsNullOrEmpty(passwordConfirm))
                {

                    if (!IsPasswordStrong(physio.Password, out List<string> errors))
                    {
                        TempData["Error"] = "Mot de passe faible : " + string.Join(", ", errors);
                        return RedirectToAction(nameof(Index));
                    }

                    if (physio.Password != passwordConfirm)
                    {
                        TempData["Error"] = "Les mots de passe ne correspondent pas.";
                        return RedirectToAction(nameof(Index));
                    }
                }

                if (ModelState.IsValid)
                {
                    var existingPhysio = await _context.Physios.FindAsync(physio.Id);
                    if (existingPhysio == null)
                    {
                        TempData["Error"] = "Kinésithérapeute introuvable.";
                        return RedirectToAction(nameof(Index));
                    }

                    existingPhysio.FirstName = physio.FirstName;
                    existingPhysio.LastName = physio.LastName;
                    existingPhysio.Email = physio.Email;
                    existingPhysio.PhoneNumber = physio.PhoneNumber;

                    if (!string.IsNullOrEmpty(physio.Password) && physio.Password.Length < 50)
                    {
                        existingPhysio.Password = PasswordHasher.HashPassword(physio.Password);
                    }

                    existingPhysio.INAMINumber = physio.INAMINumber;

                    _context.Update(existingPhysio);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Kinésithérapeute {physio.FirstName} {physio.LastName} modifié avec succès.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Données invalides. Veuillez vérifier les champs.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Error"] = "Erreur de concurrence lors de la modification.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de la modification : {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Deletes the specified physiotherapist from the database.
        /// </summary>
        /// <remarks>If the physiotherapist with the specified identifier does not exist, no deletion
        /// occurs and an error message is displayed. Success or error messages are provided via TempData for display in
        /// the redirected view.</remarks>
        /// <param name="id">The unique identifier of the physiotherapist to delete.</param>
        /// <returns>A redirect to the index view after the deletion attempt completes.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var physio = await _context.Physios.FindAsync(id);

            if (physio == null)
            {
                TempData["Error"] = "Kinésithérapeute introuvable.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Physios.Remove(physio);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Kinésithérapeute {physio.FirstName} {physio.LastName} supprimé avec succès.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de la suppression : {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Validates if a password meets the strong password requirements.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <param name="errors">Output parameter containing a list of validation errors if the password is weak.</param>
        /// <returns>True if the password is strong; otherwise, false.</returns>
        private bool IsPasswordStrong(string password, out List<string> errors)
        {
            errors = new List<string>();

            if (password.Length < 8)
                errors.Add("Le mot de passe doit contenir au moins 8 caractères");

            if (!password.Any(char.IsUpper))
                errors.Add("Le mot de passe doit contenir au moins une majuscule");

            if (!password.Any(char.IsLower))
                errors.Add("Le mot de passe doit contenir au moins une minuscule");

            if (!password.Any(char.IsDigit))
                errors.Add("Le mot de passe doit contenir au moins un chiffre");

            if (!password.Any(ch => "!@#$%^&*(),.?\":{}|<>_-+=[]\\/'`~;".Contains(ch)))
                errors.Add("Au moins un caractère spécial");

            return errors.Count == 0;
        }


    }
}
