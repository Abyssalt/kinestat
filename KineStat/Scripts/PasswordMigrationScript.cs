using KineStat.Data;
using KineStat.Services;
using Microsoft.EntityFrameworkCore;

namespace KineStat.Scripts
{
    /// <summary>
    /// Script to hash existing plain text passwords in database
    /// IMPORTANT: Run this ONCE after deploying the new password system
    /// </summary>
    public class PasswordMigrationScript
    {
        /// <summary>
        /// Hash all plain text passwords for Physios and Administrators
        /// </summary>
        public static async Task MigratePasswords(KineDbContext context)
        {
            Console.WriteLine("Starting password migration");

            var physios = await context.Physios.ToListAsync();
            int physioCount = 0;

            foreach (var physio in physios)
            {
                // Check if password is already hashed (hashed passwords are +-88 characters)
                if (physio.Password.Length < 50)
                {
                    string plainPassword = physio.Password;
                    physio.Password = PasswordHasher.HashPassword(plainPassword);
                    physioCount++;
                    Console.WriteLine($"Hashed password for Physio: {physio.Email}");
                }
            }

            // Migrate Administrator passwords
            var admins = await context.Administrators.ToListAsync();
            int adminCount = 0;

            foreach (var admin in admins)
            {
                // Check if password is already hashed
                if (admin.Password.Length < 50)
                {
                    string plainPassword = admin.Password;
                    admin.Password = PasswordHasher.HashPassword(plainPassword);
                    adminCount++;
                    Console.WriteLine($"Hashed password for Admin: {admin.Email}");
                }
            }

            await context.SaveChangesAsync();

            Console.WriteLine("Password migration completed !");
            Console.WriteLine($"- {physioCount} Physio password(s) hashed");
            Console.WriteLine($"- {adminCount} Admin password(s) hashed");
        }
    }
}