using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    [Table("QuestionLadder")]
    public class QuestionLadder : Question
    {
        public QuestionLadder()
        {
            Type = "Echelle";
        }

        public int min { get; set; } = 0;

        public int max { get; set; } = 10;



        [NotMapped]
        public int? Value
        {
            get
            {
                if (answer == null || string.IsNullOrEmpty(answer.Value))
                    return null;

                if (int.TryParse(answer.Value, out int Value))
                    return Value;

                return null;
            }
            set
            {
                if (answer == null)
                    answer = new Answer { };

                answer.Value = value?.ToString() ?? string.Empty;
            }
        }

        public override bool Validate()
        {
            if (answer == null || string.IsNullOrEmpty(answer.Value))
                return false;

            if (!int.TryParse(answer.Value, out int Value))
                return false;

            return Value >= min && Value <= max;
        }
    }
}  
