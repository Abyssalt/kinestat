
namespace KineStat.Models.DTO
{
    public class DetectedPathologyDTO
    {
        public int PatientId { get; set; }
        public int AssessmentId { get; set; }
        public int PathologyId { get; set; }
        public string PathologyName { get; set; }
        public double PathologyProbability { get; set; } 
    }
}
