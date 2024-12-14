using System.ComponentModel.DataAnnotations;
namespace e_commercedotNet.Models
{
    public class User
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
        public string Nom { get; set; } = null!;  // Utilisation de null! pour éviter l'avertissement CS8618

        [Required(ErrorMessage = "Le prénom est requis.")]
        [StringLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères.")]
        public string Prenom { get; set; } = null!;  // Utilisation de null! pour éviter l'avertissement CS8618

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress]
        public string Email { get; set; } = null!;  // Utilisation de null! pour éviter l'avertissement CS8618

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        public string Password { get; set; } = null!;  // Utilisation de null! pour éviter l'avertissement CS8618

      
    }

}
