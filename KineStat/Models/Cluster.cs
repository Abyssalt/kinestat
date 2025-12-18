using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Cluster
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }

        public string Description { get; set; }

       
        public int ?  MinNumberOfPositiveTests { get; set; }
        
      
        public double ? RVPositive {  get; set; }
   
        public double ? RVNegative { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();

        public List<Pathology> ? Pathologies  { get; set; }

    }
}
