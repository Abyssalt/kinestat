namespace KineStat.Models
{
    /// <summary>
    /// Represents a view model that associates a patient with a specific question and their corresponding answer.
    /// </summary>
    public class QuestionPatientAnswerVM 
    {
        public int PatientId { get; set; }

        public Question Question { get; set; }

        public PatientAnswer Answer { get; set; }

        public string GetComment()
        {
            if (Answer == null || Answer.Comment == null)
            {
                return "";
            }
            return Answer.Comment;
        }

        public double GetRvPositive()
        {
            return Question.RVPositive;
        }
        public double GetRvNegative()
        {
            return Question.RVNegative;
        }
    }
}
