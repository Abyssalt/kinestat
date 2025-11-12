namespace KineStat.Models
{
    public class AnswerQCM : Answer
    {
        public List<string> SelectedAnswers { get; set; } = new List<string>();
    }
}
