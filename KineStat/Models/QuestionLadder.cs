using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public class QuestionLadder : Question
    {
        public QuestionLadder()
        {
            Type = "Ladder";
        }

        public int min { get; set; } = 0;

        public int max { get; set; } = 10;
    }
}  
