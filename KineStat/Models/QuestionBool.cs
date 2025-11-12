namespace KineStat.Models
{
    /// <summary>
    /// Question de type Booléen (Oui/Non)
    /// </summary>
    public class QuestionBool : Question
    {
        public bool Answer { get; set; }

        public QuestionBool()
        {
            Type = "Bool";
        }
    }
}