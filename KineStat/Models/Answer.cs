using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string Comment { get; set; }

        public int Score { get; set; } = 0;



        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        public Answer() { }

        public int CalculateScore()
        {
            return 0;
        }

        public bool ReveleRedFlag()
        {
            return Score >= Question.RVPositif && Question.RVPositif > 0;
        }

        public void AddComment(string texte)
        {
            if (string.IsNullOrEmpty(Comment))
                Comment = texte;
            else
                Comment += "\n" + texte;
        }
    }
}