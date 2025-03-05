using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using miAplicacion.Models;
using miAplicacion.Extensions;

namespace miAplicacion.Pages
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string TipoOferta { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
        public string Estado { get; set; }
        public string ImagenUrl { get; set; }
    }

    public class GestionProductosModel : PageModel
    {
        private readonly ILogger<GestionProductosModel> _logger;
        [BindProperty]
        public List<Producto> Productos { get; set; }
        
        [TempData]
        public string MensajeExito { get; set; }

        public GestionProductosModel(ILogger<GestionProductosModel> logger)
        {
            _logger = logger;
            // Datos de ejemplo - En un caso real, esto vendría de una base de datos
            Productos = new List<Producto>
            {
                new Producto { 
                    Id = 1, 
                    Nombre = "Camiseta Premium", 
                    TipoOferta = "2x1", 
                    Cantidad = 50, 
                    Precio = 29.99M,
                    Categoria = "Ropa",
                    Estado = "En Stock",
                    ImagenUrl = "https://via.placeholder.com/50"
                },
                new Producto { 
                    Id = 2, 
                    Nombre = "Pantalón Vaquero", 
                    TipoOferta = "30% OFF", 
                    Cantidad = 30, 
                    Precio = 59.99M,
                    Categoria = "Ropa",
                    Estado = "Bajo Stock",
                    ImagenUrl = "https://via.placeholder.com/50"
                },
                new Producto { 
                    Id = 3, 
                    Nombre = "Zapatillas Running", 
                    TipoOferta = "Sin oferta", 
                    Cantidad = 25, 
                    Precio = 89.99M,
                    Categoria = "Calzado",
                    Estado = "En Stock",
                    ImagenUrl = "https://via.placeholder.com/50"
                },
                new Producto { 
                    Id = 4, 
                    Nombre = "Gorra Deportiva", 
                    TipoOferta = "50% OFF", 
                    Cantidad = 40, 
                    Precio = 19.99M,
                    Categoria = "Accesorios",
                    Estado = "En Stock",
                    ImagenUrl = "https://via.placeholder.com/50"
                }
            };
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Simulamos una actualización exitosa
            MensajeExito = "¡Inventario actualizado correctamente!";
            return Page();
        }

        public IActionResult OnPostAgregarAlCarrito(int productoId, int cantidad)
        {
            var producto = Productos.FirstOrDefault(p => p.Id == productoId);
            if (producto == null)
            {
                return NotFound();
            }

            var carritoItems = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();
            
            var item = new CarritoItem
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Cantidad = cantidad,
                Precio = producto.Precio,
                TipoOferta = producto.TipoOferta
            };

            carritoItems.Add(item);
            HttpContext.Session.SetObject("Carrito", carritoItems);

            MensajeExito = $"{producto.Nombre} agregado al carrito";
            return RedirectToPage();
        }
    }
} 