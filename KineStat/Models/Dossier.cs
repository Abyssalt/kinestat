using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Dossier
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        public string Titre { get; set; }

        [Required]
        public DateTime DateOuverture { get; set; } = DateTime.Today;

        public string? Notes { get; set; }

        // 🔗 Relation Patient
        [Required]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}