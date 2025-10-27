using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static KineStat.Models.QuestionModel;

namespace KineStat.Models
{
    /// <summary>
    /// Représente une réponse donnée à une question
    /// </summary>
    [Table("Answer")]
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "La valeur de la réponse est obligatoire")]
        public string Value { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateReponse { get; set; } = DateTime.Now;

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Range(0, 100)]
        public int Score { get; set; } = 0;

  
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }


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
    }
}
