using System.ComponentModel.DataAnnotations;

namespace miAplicacion.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }

        public bool RecordarMe { get; set; }
    }
} 