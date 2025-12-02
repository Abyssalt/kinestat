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
        public virtual List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
