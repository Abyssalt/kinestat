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
        public DbSet<Physio> Physio { get; set; }
        public DbSet<Cluster> Cluster { get; set; }
        public DbSet<Bilan> Bilan { get; set; }
        public DbSet<QuestionBool> QuestionBool { get; set; }
        public DbSet<QuestionQCM> QuestionQCM { get; set; }
        public DbSet<QuestionLadder> QuestionLadder { get; set; }
        public DbSet<AnswerBool> AnswerBool { get; set; }
        public DbSet<AnswerQCM> AnswerQCM { get; set; }
        public DbSet<AnswerLadder> AnswerLadder { get; set; }
        public DbSet<Redflag> Redflag { get; set; }
        public DbSet<Pathology> Pathology { get; set; }
        public DbSet<Category> Category { get; set; }


    }
}
