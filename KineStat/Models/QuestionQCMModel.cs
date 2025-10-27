using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static KineStat.Models.QuestionModel;

namespace KineStat.Models
{
    /// <summary>
    /// Question de type QCM (Questionnaire à Choix Multiples)
    /// </summary>
    [Table("QuestionQCM")]
    public class QuestionQCMModel : Question
    {
        public QuestionQCMModel()
        {
            Type = "QCM";
            ListAnswers = new List<string>();
        }

        [Required]
        [NotMapped] 
        public List<string> ListAnswers { get; set; }

       

        public override bool Validate()
        {
            if (answer == null || string.IsNullOrEmpty(answer.Value))
                return false;

            return ListAnswers.Contains(answer.Value);
        }
    }
}
