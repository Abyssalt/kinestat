using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Redflag
    {
        [Key]
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public int PatientId { get; set; }
        public double Value {  get; set; }
        public int AssessmentId;

        public virtual Patient Patient { get; set; }
        public virtual Category Category { get; set; }
    }
}