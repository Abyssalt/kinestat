using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class ClinicalData //This class is used to store values of each axe of the patient's clinical profile
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int CategoryId { get; set; }
        public double Value { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Category Category { get; set; }

    }
}
