using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace miAplicacion.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Cerrar la sesión de autenticación
            await HttpContext.SignOutAsync("Cookies");

            _logger.LogInformation("Usuario cerró sesión");

            // Redirigir al login
            return RedirectToPage("/Index");
        }
    }
} 