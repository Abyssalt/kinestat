using System.ComponentModel.DataAnnotations;

namespace KineStat.Models.ViewModels
{
    /// <summary>
    /// ViewModel for the login form
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "L'adresse email est requise")]
        [EmailAddress(ErrorMessage = "Adresse email invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}