using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    /// <summary>
    /// Represents the association between a pathology and a medical context, including a value indicating the relevance
    /// or weight of the pathology within that context.
    /// </summary>
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


