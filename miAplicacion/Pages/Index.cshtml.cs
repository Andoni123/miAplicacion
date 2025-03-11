using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using miAplicacion.Models;
using miAplicacion.Services;

namespace miAplicacion.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    // Lista de usuarios (en producción esto estaría en una base de datos)
    private static readonly List<Usuario> _usuarios = new()
    {
        Usuario.CrearUsuario("admin", "admin123", "admin@example.com", "Admin"),
        Usuario.CrearUsuario("usuario", "user123", "user@example.com", "Usuario")
    };

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToPage("/GestionProductos");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Username == "admin" && Password == "admin123")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToPage("/GestionProductos");
        }

        ErrorMessage = "Usuario o contraseña incorrectos";
        return Page();
    }
}
