using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public abstract class Answer
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string Comment { get; set; }

        public int Score { get; set; } = 0;

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        public void AddComment(string texte)
        {
            if (string.IsNullOrEmpty(Comment))
                Comment = texte;
            else
                Comment += "\n" + texte;
        }
    }
}