using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    /// <summary>
    /// Represents a prior context entity that associates a value with a specific medical context and category.
    /// </summary>
    public class PriorContext 
    {
        [Key]
        public int Id { get; set; }

        public double Value { get; set; } = 0;

        public int CategoryId { get; set; }

        public int MedicalContextId { get; set; }

        public virtual MedicalContext MedicalContext { get; set; }

        public virtual Category Category { get; set; }
    }
}
