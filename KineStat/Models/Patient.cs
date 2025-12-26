using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    /// <summary>
    /// Specifies the gender options available for classification.
    /// </summary>
    public enum Gender
    {
        Homme,
        Femme,
        Autre
    }

    /// <summary>
    /// Specifies the status of a patient within the system.
    /// </summary>
    public enum PatientStatus
    {
        Actif,
        Terminé,
        Inactif,
        Abandonné,
    }

    /// <summary>
    /// Specifies supported countries for regional operations.
    /// </summary>
    public enum Country
    {
        Belgique,
        France,
        Luxembourg,
        Suisse
    }

    /// <summary>
    /// Represents a patient and their associated personal, medical, and administrative information within the
    /// healthcare system.
    /// </summary>
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Date), Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        public int? Weight { get; set; }

        public int? Height { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public string SocialSecurityNumber { get; set; }

        [Required]
        public Country Country { get; set; }


        //Medical Informations
        public string? Profession { get; set; }
        public string? PhysicalActivities { get; set; }
        public string? MedicalHistory { get; set; }
        public string? ActualMedication { get; set; }

        public PatientStatus Status { get; set; }

        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public string? Address { get; set; }

        [Required]
        public int PhysioId { get; set; }

        public Physio? Physio { get; set; }

        public ICollection<Folder>? Folders { get; set; } = new List<Folder>();

        public ICollection<Assessment>? Assessments { get; set; } = new List<Assessment>();

        public virtual ICollection<PatientAnswerTests>? Responses { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsAnonymized { get; set; } = false;

        public DateTime? AnonymizedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? InactiveSinceDate { get; set; }
    }
}