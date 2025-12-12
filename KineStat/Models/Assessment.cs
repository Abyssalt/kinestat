using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{

    using System.ComponentModel.DataAnnotations;

    public enum AssessmentStatus
    {
        EnCours,
        Cloture
    }

    public class Assessment
    {
        [Key]
        public int Id { get; set; }

        [Required, DataType(DataType.Date), Column(TypeName = "date")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        [Required]
        public int MedicalContextId { get; set; }

        [Required]
        public int PhysioId { get; set; }

        public virtual Physio Physio { get; set; }

        public virtual ICollection<Redflag> RedFlagsDetected { get; set; } = new List<Redflag>();

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

        public virtual MedicalContext MedicalContext { get; set; }

        [Required]
        public int DossierId { get; set; }

        public Dossier Dossier { get; set; }

        public AssessmentStatus Status { get; set; }

        public double? RedFlagsPercentage { get; set; }

    }
}