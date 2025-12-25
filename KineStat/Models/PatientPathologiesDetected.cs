using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    /// <summary>
    /// Represents a record of a pathology detected for a specific patient during an assessment, including the
    /// probability of detection and related entities.
    /// </summary>
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
