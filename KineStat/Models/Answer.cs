using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    /// <summary>
    /// Represents an answer to a question, including its title, score, and association with a specific question.
    /// </summary>
    /// <remarks>The Answer class is typically used in applications that manage questions and answers, such as
    /// surveys, quizzes, or forums. Each instance corresponds to a single answer and maintains a reference to the
    /// related question. The class supports entity framework attributes for database mapping.</remarks>
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