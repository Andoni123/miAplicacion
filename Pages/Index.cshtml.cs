using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        [BindProperty]
        public string Usuario { get; set; } = "";
        
        [BindProperty]
        public string Password { get; set; } = "";
        
        public string Error { get; set; } = "";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnPost()
        {
            if (Usuario == "admin" && Password == "admin123")
            {
                return RedirectToPage("/GestionProductos");
            }
            
            Error = "Usuario o contraseña incorrectos";
            return Page();
        }
    }
} 