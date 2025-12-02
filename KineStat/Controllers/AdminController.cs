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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var physios = await _context.Physios
                .Include(p => p.Patients)
                .OrderBy(p => p.LastName)
                .ToListAsync();

            return View(physios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Physio physio)
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Physio physio)
        {
            try
            {
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
    }
}
