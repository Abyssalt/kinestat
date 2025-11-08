using Microsoft.EntityFrameworkCore;
using KineStat.Models;

namespace KineStat.Data
{
    public class KineDbContext : DbContext
    {
        public KineDbContext(DbContextOptions<KineDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Kine> Kine { get; set; }
        public DbSet<Cluster> Cluster { get; set; }
        public DbSet<Bilan> Bilan { get; set; }
        public DbSet<QuestionBool> QuestionBool { get; set; }
        public DbSet<QuestionQCM> QuestionQCM { get; set; }
        public DbSet<QuestionLadder> QuestionLadder { get; set; }
        public DbSet<RedFlags> Redflags { get; set; }
        public DbSet<Answer> Answer { get; set; }
    }
}
