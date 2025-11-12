using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Bilan
    {
        [Key]
        public int Id { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        [Required]
        public int PhysioId { get; set; }
        public virtual Physio Physio { get; set; }

        public virtual ICollection<Redflag> RedFlagsDetected { get; set; } = new List<Redflag>();
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}