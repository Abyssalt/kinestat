using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{

    /// <summary>
    /// Represents a folder containing patient information, assessments, and related metadata within the medical records
    /// system.
    /// </summary>
    public class Folder
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        public string Title { get; set; }

        [Required]
        public DateTime OpeningDate { get; set; }

        public string? Notes { get; set; }

        [Required]
        public int PatientId { get; set; }

        public Patient? Patient { get; set; }

        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}