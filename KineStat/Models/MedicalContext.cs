using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class MedicalContext
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<PriorContext> PriorContexts { get; set; }


    }
}
