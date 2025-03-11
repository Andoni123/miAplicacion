using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace miAplicacion.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string Usuario { get; set; } = "";
    
    [BindProperty]
    public string Password { get; set; } = "";
    
    public string Error { get; set; } = "";

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