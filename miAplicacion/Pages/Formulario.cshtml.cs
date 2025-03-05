using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace miAplicacion.Pages
{
    public class FormularioModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Por favor, introduce un email válido")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [MinLength(10, ErrorMessage = "El mensaje debe tener al menos 10 caracteres")]
        public string Mensaje { get; set; }

        public string ResultadoMensaje { get; set; }

        public void OnGet()
        {
            // Esta función se ejecuta cuando se carga la página
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Aquí puedes procesar los datos del formulario
            ResultadoMensaje = $"¡Gracias {Nombre}! Hemos recibido tu mensaje y te contactaremos en {Email}";

            // Limpia el formulario
            ModelState.Clear();
            return Page();
        }
    }
} 