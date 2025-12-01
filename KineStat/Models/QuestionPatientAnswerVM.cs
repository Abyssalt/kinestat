namespace KineStat.Models
{
    //This class is used as a ViewModel to share information between the controller and View
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
