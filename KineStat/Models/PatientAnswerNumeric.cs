namespace KineStat.Models
{
    /// <summary>
    /// Represents a numeric answer provided by a patient in response to a question.
    /// </summary>
    public class PatientAnswerNumeric : PatientAnswer
    {
        public double Value { get; set; }

    }
}
