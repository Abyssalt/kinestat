using System.ComponentModel.DataAnnotations.Schema;
using static KineStat.Models.QuestionModel;

namespace KineStat.Models
{
    /// <summary>
    /// Question de type Booléen (Oui/Non)
    /// </summary>
    [Table("QuestionBool")]
    public class QuestionBool : Question
    {
        public QuestionBool()
        {
            Type = "Bool";
        }

        public override bool Valider()
        {
            if (Answer == null || string.IsNullOrEmpty(Answer.Value))
                return false;

            return Answer.Value.ToLower() == "oui" ||
                   Answer.Value.ToLower() == "non" ||
                   Answer.Value == "true" ||
                   Answer.Value == "false";
        }

        // Helper pour obtenir la réponse booléenne
        public bool? GetAnswerBool()
        {
            if (Answer == null || string.IsNullOrEmpty(Answer.Value))
                return null;

            var Value = Answer.Value.ToLower();
            if (Value == "oui" || Value == "true")
                return true;
            if (Value == "non" || Value == "false")
                return false;

            return null;
        }
    }
}
