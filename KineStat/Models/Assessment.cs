using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{

    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Specifies the status of an assessment process.
    /// </summary>
    /// <remarks>Use this enumeration to indicate whether an assessment is currently in progress or has been
    /// closed. The values correspond to distinct stages in the assessment lifecycle.</remarks>
    public enum AssessmentStatus
    {
        EnCours,
        Cloture
    }

    /// <summary>
    /// Represents a clinical assessment performed for a patient, including associated context, practitioner, and
    /// related questions.
    /// </summary>
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

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

        public virtual MedicalContext MedicalContext { get; set; }

        [Required]
        public int FolderId { get; set; }

        public Folder Folder { get; set; }

        public AssessmentStatus Status { get; set; }

        public double? RedFlagsPercentage { get; set; }

    }
}