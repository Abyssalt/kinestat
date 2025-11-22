using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class PriorContext // This class represents the prior of each category according to the current medical context
    {
        [Key]
        public int Id { get; set; }

        public double Value { get; set; } = 0;

        public int CategoryId;

        public int MedicalContextId;

        public virtual MedicalContext MedicalContext { get; set; }
        public virtual Category Category { get; set; }

    }
}
