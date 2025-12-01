using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public abstract class PatientAnswer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int AssessmentId { get; set; }

        public string? Comment { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual Question Question { get; set; }

        public virtual Assessment Assessment { get; set; }
    }
}
