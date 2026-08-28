using KineStat.Models;
using KineStat.Services;
using Microsoft.EntityFrameworkCore;

namespace KineStat.Data
{
    /// <summary>
    /// Applies pending EF Core migrations, runs the demo population script,
    /// then guarantees documented demo credentials exist.
    /// Order matters: populate.sql inserts Physios with explicit Ids (0 and 1),
    /// so no account may be created before it runs on a fresh database.
    /// </summary>
    public static class DbSeeder
    {
        public static void MigrateAndSeed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<KineDbContext>();

            context.Database.Migrate();

            if (!context.Patients.Any())
            {
                var sqlPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "populate.sql");
                if (File.Exists(sqlPath))
                {
                    var sql = File.ReadAllText(sqlPath);
                    using var transaction = context.Database.BeginTransaction();
                    context.Database.ExecuteSqlRaw(sql);
                    transaction.Commit();
                }
            }

            // if no seed script, insert these two test account
            if (!context.Administrators.Any())
            {
                context.Administrators.Add(new Administrator
                {
                    FirstName = "Admin",
                    LastName = "KineStat",
                    Email = "admin@kinestat.local",
                    Password = PasswordHasher.HashPassword("Admin123!")
                });
            }

            if (!context.Physios.Any())
            {
                context.Physios.Add(new Physio
                {
                    FirstName = "Demo",
                    LastName = "Physio",
                    Email = "physio@kinestat.local",
                    PhoneNumber = "0400000000",
                    INAMINumber = 12345678901,
                    Password = PasswordHasher.HashPassword("Physio123!")
                });
            }

            context.SaveChanges();
        }
    }
}