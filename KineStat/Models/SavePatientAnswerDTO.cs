namespace KineStat.Models
{
    /// <summary>
    /// This class is a DTO  used to pass patient responses from the view to the controller
    /// Supports different types of answers: boolean, numeric, and optional comments
    /// </summary>
    public class SavePatientAnswerDTO
    {
        public int PatientId { get; set; }

        public int QuestionId { get; set; }

        public bool ? BoolValue { get; set; }

        public double ? NumericValue { get; set; }

        public string ? Comment { get; set;  }

        public int AssessmentId { get; set; }
    }
}
