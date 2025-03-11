using miAplicacion.Services;

namespace miAplicacion.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = "Usuario"; // Puede ser "Admin" o "Usuario"
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public static Usuario CrearUsuario(string username, string password, string email, string rol = "Usuario")
        {
            return new Usuario
            {
                Username = username,
                PasswordHash = PasswordHasher.HashPassword(password),
                Email = email,
                Rol = rol,
                FechaRegistro = DateTime.Now
            };
        }
    }
} 