/*
Ne lancez pas ce script si vous ne savez pas ce qu'il fait !
Il permet de convertir les mdps clairs stockés en BD en hash.
C'est donc juste temporaire, vu que pour le moment les mdps sont stockés en clair.

Il est néanmoins un petit peu protégé, pour la longueur des caractères (qui dit que si un mdp > 50 alors il est hashé, sinon il est en clair).

Pour le lancer, vous devez :
    - Rebuild le projet
    - Vous rendre ici : https://localhost:7133/Maintenance/MigratePasswords
    - Surtout le supprimer après utilisation (car nous ne devons plus le lancer après)
*/

using System.Net;
using KineStat.Data;
using KineStat.Migrations;
using KineStat.Scripts;
using Microsoft.AspNetCore.Mvc;

namespace KineStat.Controllers
{
    /// <summary>
    /// Maintenance controller for administrative tasks
    /// WARNING: This controller should be deleted and not used after password migration !
    /// </summary>
    public class MaintenanceController : Controller
    {
        private readonly KineDbContext _context;

        public MaintenanceController(KineDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Endpoint to migrate plain text passwords to hashed passwords
        /// ACCESS THIS ONCE: /Maintenance/MigratePasswords
        /// Then DELETE this entire controller!
        /// </summary>
        public async Task<IActionResult> MigratePasswords()
        {
            try
            {
                await PasswordMigrationScript.MigratePasswords(_context);

                return Content(
                    "✅ Migration des mots de passe terminée avec succès !\n\n" +
                    "Consultez la console de Visual Studio pour voir les détails.\n\n" +
                    "⚠️ IMPORTANT : Supprimez maintenant le fichier MaintenanceController.cs !",
                    "text/plain");
            }
            catch (Exception ex)
            {
                return Content(
                    $"Erreur lors de la migration :\n\n{ex.Message}\n\n{ex.StackTrace}",
                    "text/plain");
            }
        }
    }
}