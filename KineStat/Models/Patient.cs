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
        Actif,
        Terminé,
        Inactif,
        Abandonné,
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


        //Medical Informations
        public string? Profession { get; set; }
        public string? ActivitesPhysiques { get; set; }
        public string? AntecedentsMedicaux { get; set; }
        public string? MedicationActuelle { get; set; }

        public PatientStatus Status { get; set; }

        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public string? Address { get; set; }

        [Required]
        public int PhysioId { get; set; }

        public Physio? Physio { get; set; }

        public ICollection<Dossier>? Dossiers { get; set; } = new List<Dossier>();

        public ICollection<Assessment>? Assessments { get; set; } = new List<Assessment>();

        public virtual ICollection<PatientAnswerTests>? Responses { get; set; }
    }
}