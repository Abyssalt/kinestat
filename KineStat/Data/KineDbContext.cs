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
        public DbSet<Physio> Physios { get; set; }
        public DbSet<Cluster> Cluster { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<QuestionBool> QuestionBools { get; set; }
        public DbSet<QuestionQCM> QuestionQCMs { get; set; }
        public DbSet<QuestionLadder> QuestionLadders { get; set; }
        public DbSet<Redflag> Redflags { get; set; }
        public DbSet<Pathology> Pathologies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet <Answer> Answers { get; set; }
        public DbSet <ClinicalData> ClinicalDatas { get; set; }
        public DbSet<MedicalContext> MedicalContexts { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<PatientAnswerBool> PatientAnswerBools { get; set; }
        public DbSet<PatientAnswerNumeric> PatientAnswerNumerics { get; set; }
        public DbSet<PatientAnswerQCM> PatientAnswerQCMs { get; set; }
        public DbSet<PriorContext> PriorContexts { get; set; }
        public DbSet<Socrate> Socrates { get; set; }
        public DbSet<RedflagThreshold> RedflagThresholds { get; set; }

        public DbSet<Dossier> Dossiers { get; set; }



    }
}
