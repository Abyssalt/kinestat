using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    /// <summary>
    /// Represents a question that defines a numeric range with minimum and maximum values.
    /// </summary>
    public class QuestionLadder : Question
    {
        public int Min { get; set; } = 0;

        public int Max { get; set; } = 10;
    }
}  
