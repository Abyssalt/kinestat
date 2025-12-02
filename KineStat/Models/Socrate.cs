using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Socrate
    {
        [Key] 
        public int Id { get; set; }

        public int AssessmentId { get; set; }

        public int PatientId { get; set; }

        public string? Site {  get; set; }

        public string? Onset { get; set; }

        public string? Character { get; set; }

        public string? Radiation { get; set; }

        public string? Association { get; set; }

        public string? Timing {  get; set; }

        public string? ExacerbatingFactor { get; set; }

        public string? RelievingFactor { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual Assessment Assessment { get; set; }
    }
}
