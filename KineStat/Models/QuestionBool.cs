using System.ComponentModel.DataAnnotations.Schema;
using static KineStat.Models.Question;

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

        public override bool Validate()
        {
            if (answer == null || string.IsNullOrEmpty(answer.Value))
                return false;

            
            return answer.Value == "true" ||
                   answer.Value == "false";
        }

        public bool? GetAnswerBool()
        {
            if (answer == null || string.IsNullOrEmpty(answer.Value))
                return null;

            var Value = answer.Value.ToLower();
            if (Value == "oui" || Value == "true")
                return true;
            if (Value == "non" || Value == "false")
                return false;

            return null;
        }

        public bool? isPositive()
        {
            return GetAnswerBool();
        }
    }
}
