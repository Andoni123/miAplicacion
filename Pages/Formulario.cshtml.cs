using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages
{
    public class FormularioModel : PageModel
    {
        public FormularioModel()
        {
            Nombre = string.Empty;
            Email = string.Empty;
            Mensaje = string.Empty;
            ResultadoMensaje = string.Empty;
        }

        [Required(ErrorMessage = "El nombre es requerido")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El mensaje es requerido")]
        public required string Mensaje { get; set; }

        public required string ResultadoMensaje { get; set; }

        // ... resto del código ...
    }
} 