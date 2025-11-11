using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Categorie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        public double Prior { get; set; }

        public List<Question> listeQuestions { get; set; } = new List<Question>(); // Liste de questions reprises, c'est donc un cluster ? A voir si le nom convient, vu qu'on a déjà une classe Cluster, j'ai pris un autre nom.

        /// <summary>
        /// Retourne le nombre de questions que contient la liste (utile pour montrer au kiné la taille de cette liste, et donc le temps qu'il va devoir prendre pour toutes les questions)
        /// </summary>
        public int GetNbQuestions()
        {
            return listeQuestions.Count;
        }
    }
}