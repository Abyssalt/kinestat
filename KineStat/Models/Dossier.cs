using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public enum FolderStatus
    {
        Cloturé,
        EnCours
    }

    public class Dossier
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        public string Titre { get; set; }

        [Required]
        public DateTime DateOuverture { get; set; }

        public string? Notes { get; set; }

        public FolderStatus Status { get; set; }

        [Required]
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}