using System.ComponentModel.DataAnnotations;

namespace KineStat.Models
{
    /// <summary>
    /// Represents a treating physician in the KineStat system.
    /// Stores the doctor's identification information, including their INAMI number (National Institute for Health and Disability Insurance),
    /// and maintains relationships with patients for whom they are the treating physician.
    /// </summary>
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Le numéro INAMI est requis")]
        [StringLength(20, ErrorMessage = "Le numéro INAMI ne peut pas dépasser 20 caractères")]
        [RegularExpression(@"^\d{11}$|^\d-\d{5}-\d{2}-\d{3}$", ErrorMessage = "Le numéro INAMI doit être au format valide (11 chiffres ou X-XXXXX-XX-XXX)")]
        public string NumeroINAMI { get; set; }

        // Navigation property - Liste des patients associés à ce médecin
        public ICollection<Patient>? Patients { get; set; } = new List<Patient>();

        // Propriété calculée pour affichage
        public string FullName => $"{FirstName} {LastName}";
    }
}
