using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace miAplicacion.Pages
{
    public class LoginPageModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        
        [BindProperty]
        public string Password { get; set; }
        
        [BindProperty]
        public bool RecordarMe { get; set; }

        public void OnGet()
        {
        }
    }
} 