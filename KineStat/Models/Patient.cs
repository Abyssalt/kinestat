using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineStat.Models
{
    public enum Gender
    {
        Homme,
        Femme,
        Autre
    }

    public enum PatientStatus
    {
        Pending = 0,
        Active = 1,
        Completed = 2,
        Suspended = 3,
        Abandoned = 4
    }

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

        public PatientStatus PatientStatus { get; set; } = PatientStatus.Active;

        public string? DoctorName { get; set; }

        public string? DoctorINAMI { get; set; }

        public string? Address { get; set; }

        [Required]
        public int PhysioId { get; set; }

        public Physio? Physio { get; set; }

        public ICollection<Dossier>? Dossiers { get; set; } = new List<Dossier>();

        public ICollection<Assessment>? Assessments { get; set; } = new List<Assessment>();
    }
}