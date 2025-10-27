using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Cluster
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public List<QuestionModel> questions { get; set; } = new List<QuestionModel>();
    }
}
