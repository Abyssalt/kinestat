using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    /// <summary>
    /// Represents the clinical exam's data associated with a patient.
    /// </summary>
    public class ClinicalData
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int? AssessmentId { get; set; }
        public int CategoryId { get; set; }
        public double Value { get; set; }
        public virtual Patient Patient { get; set; }
        public Assessment? Assessment { get; set; }
        public virtual Category Category { get; set; }

    }
}
