using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static KineStat.Models.QuestionModel;

namespace KineStat.Models
{
    /// <summary>
    /// Question de type QCM (Questionnaire à Choix Multiples)
    /// </summary>
    [Table("QuestionQCM")]
    public class QuestionQCM : Question
    {
        public QuestionQCM()
        {
            Type = "QCM";
            ListeAnswers = new List<string>();
        }

        [Required]
        [NotMapped] 
        public List<string> ListeAnswers { get; set; }

       

        public override bool Validate()
        {
            if (answer == null || string.IsNullOrEmpty(answer.Value))
                return false;

            return ListeAnswers.Contains(answer.Value);
        }
    }
}
