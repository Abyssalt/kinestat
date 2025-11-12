using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static KineStat.Models.Question;

namespace KineStat.Models
{
    /// <summary>
    /// Question de type QCM (Questionnaire à Choix Multiples)
    /// </summary>
    public class QuestionQCM : Question
    {
        public QuestionQCM()
        {
            Type = "QCM";
            ListAnswers = new List<string>();
        }

        public List<string> ListAnswers { get; set; }
    }
}
