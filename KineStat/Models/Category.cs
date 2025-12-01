using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Prior {  get; set; }

        public List<Cluster> Clusters { get; set; }
        public virtual ICollection<Pathology> Pathologies { get; set; } = new List<Pathology>();

        /// <summary>
        /// Retourne le nombre de cluster que contient la liste (utile pour montrer au kiné la taille de cette liste, et donc le temps qu'il va devoir prendre pour toutes les questions)
        /// </summary>
        public int getNbClusters()
        {
            return Clusters.Count;
        }
    }
}