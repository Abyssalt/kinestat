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

        [Range(0, 500)]
        public double Weight { get; set; }

        [Range(0, 500)]
        public double Height { get; set; }

        [Required]
        public Gender Genre { get; set; }

        public string? DoctorName { get; set; }

        public string? DoctorINAMI { get; set; }

        public string? Address { get; set; }

        [Required]
        public int PhysioId { get; set; }

        public Physio? Physio { get; set; }

        public ICollection<Bilan> Bilans { get; set; } = new List<Bilan>();
    }
}