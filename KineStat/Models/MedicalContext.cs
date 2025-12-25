using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    /// <summary>
    /// Represents a medical context, including its identifier, name, and associated prior contexts.
    /// </summary>
    public class MedicalContext
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<PriorContext> PriorContexts { get; set; }


    }
}
