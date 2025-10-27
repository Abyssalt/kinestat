using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    [Table("QuestionLadder")]
    public class QuestionLadderModel : QuestionModel.Question
    {
        public QuestionLadderModel()
        {
            Type = "Echelle";
            min = 0;
            max = 10;
        }

        [Required]
        [Range(0, 100)]
        public int min { get; set; } = 0;

        [Required]
        [Range(0, 100)]
        public int max { get; set; } = 10;



        [NotMapped]
        public int? Valeur
        {
            get
            {
                if (answer == null || string.IsNullOrEmpty(answer.Valeur))
                    return null;

                if (int.TryParse(answer.Valeur, out int valeur))
                    return valeur;

                return null;
            }
            set
            {
                if (answer == null)
                    answer = new Answer { QuestionId = this.Id };

                answer.Valeur = value?.ToString() ?? string.Empty;
            }
        }

        public override bool Validate()
        {
            if (answer == null || string.IsNullOrEmpty(answer.Valeur))
                return false;

            if (!int.TryParse(answer.Valeur, out int valeur))
                return false;

            return valeur >= min && valeur <= max;
        }
    }
}
}
