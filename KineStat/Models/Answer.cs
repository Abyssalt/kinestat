using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int Score { get; set; } = 0;

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}