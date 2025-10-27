using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    public class Kine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string FistName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public List<Patient> Patients { get; set; } = new List<Patient>();

        public List<Bilan> Bilans { get; set; } = new List<Bilan>();
    }
}
