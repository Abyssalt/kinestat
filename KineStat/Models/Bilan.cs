using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    // il manque des attributs 
    public class Bilan
    {

        [Key]
        public int Id { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        public int PatientId { get; set; }
        [Required]
        public int KineId { get; set; }

        public List<RedFlags> RedFlagsDetected { get; set; }
    }
}
