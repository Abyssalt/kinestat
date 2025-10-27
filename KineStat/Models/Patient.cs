using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    // il manque l'attribut genre 
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string FisrtName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime BirthDate{ get; set; }

        [Range(0,500)]
        public double Weight { get; set; }

        [Range(0,500)]
        public double Height { get; set; }


    }
}
