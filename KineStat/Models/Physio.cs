using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    /// <summary>
    /// Represents a physiotherapist (kinesiotherapist) in the KineStat system.
    /// Stores the physiotherapist's identification, contact information, authentication credentials,
    /// and maintains relationships with their patients.
    /// </summary>
    public class Physio
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

        [Required]
        public String Password { get; set; }

        [Required]
        public long INAMINumber { get; set; }

        public List<Patient> Patients { get; set; } = new List<Patient>();
    }
}
