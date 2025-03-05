using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace miAplicacion.Pages
{
    public class RegisterPageModel : PageModel
    {
        [BindProperty]
        public string Nombre { get; set; }
        
        [BindProperty]
        public string Email { get; set; }
        
        [BindProperty]
        public string Password { get; set; }
        
        [BindProperty]
        public string ConfirmarPassword { get; set; }

        public void OnGet()
        {
        }
    }
}