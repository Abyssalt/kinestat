using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public class QuestionLadder : Question
    {
        public int Min { get; set; } = 0;

        public int Max { get; set; } = 10;
    }
}  
