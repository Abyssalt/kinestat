using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class PriorContextPathology {
        
        [Key]
        public int Id { get; set; }

        public double Value { get; set; } = 0;

        public int PathologyId { get; set; }

        public int MedicalContextId { get; set; }

        public virtual MedicalContext MedicalContext { get; set; }

        public virtual Pathology Pathology { get; set; }
    }
}


