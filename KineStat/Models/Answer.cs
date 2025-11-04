using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static KineStat.Models.Question;

namespace KineStat.Models
{
    /// <summary>
    /// Représente une réponse donnée à une question
    /// </summary>
    [Table("Answer")]
    public class Answer
    {

        public int Id { get; set; }

        public string Value { get; set; }

        public string Comment { get; set; }

        public int Score { get; set; } = 0;

        
        public virtual Question Question { get; set; }

        public Answer() { }

        /// <summary>
        /// Calcule le score en fonction de la question et du RV
        /// </summary>
        public int CalculateScore()
        {
            return 0;
        }

        /// <summary>
        /// Vérifie si la réponse révèle un potentiel red flag
        /// </summary>
        public bool ReveleRedFlag()
        {
            return Score >= Question.RVPositif && Question.RVPositif > 0;
        }

        /// <summary>
        /// Ajoute une observation
        /// </summary>
        public void AddComment(string texte)
        {
            if (string.IsNullOrEmpty(Comment))
                Comment = texte;
            else
                Comment += "\n" + texte;
        }
    }
}
