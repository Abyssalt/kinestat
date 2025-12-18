using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class PatientPathologiesDetected
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int AssessmentId { get; set; }  
        [Required]
        public int PathologyId { get; set; }    
        [Required]
        public double PathologyProbability { get; set; }    

        public Pathology Pathology { get; set; }
        public Patient Patient { get; set; }

    }
}
