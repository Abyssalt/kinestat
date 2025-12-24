using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    /// <summary>
    /// Represents a SOCRATES pain assessment in the KineStat system.
    /// SOCRATES is a mnemonic used in medical practice to systematically evaluate and document pain characteristics.
    /// Each property corresponds to one aspect of the pain assessment framework.
    /// </summary>
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
