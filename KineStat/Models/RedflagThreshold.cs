using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class RedflagThreshold
    {
        [Key]
        public int Id { get; set; }

        public double ThresholdPercentage { get; set; }
        [Required]
        public string Description { get; set; }



    }
}
