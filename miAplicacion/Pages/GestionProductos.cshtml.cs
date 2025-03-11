using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using miAplicacion.Models;
using miAplicacion.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace miAplicacion.Pages
{
    [Authorize]
    public class GestionProductosModel : PageModel
    {
        private readonly ILogger<GestionProductosModel> _logger;

        public GestionProductosModel(ILogger<GestionProductosModel> logger)
        {
            _logger = logger;
            // Inicializar productos de ejemplo
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

        [BindProperty]
        public List<Producto> Productos { get; set; }

        [TempData]
        public string MensajeExito { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostAgregarAlCarrito(int productoId, string nombre, decimal precio, string tipoOferta, string imagenUrl, int cantidad)
        {
            try
            {
                var carritoItems = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();
                
                var itemExistente = carritoItems.FirstOrDefault(i => i.Id == productoId);
                if (itemExistente != null)
                {
                    itemExistente.Cantidad += cantidad;
                }
                else
                {
                    carritoItems.Add(new CarritoItem
                    {
                        Id = productoId,
                        Nombre = nombre,
                        Precio = precio,
                        Cantidad = cantidad,
                        TipoOferta = tipoOferta,
                        ImagenUrl = imagenUrl
                    });
                }

                HttpContext.Session.SetObject("Carrito", carritoItems);
                TempData["MensajeExito"] = $"Se agregó {cantidad} {nombre}(s) al carrito";
                
                return RedirectToPage("/Carrito");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al agregar al carrito: {ex.Message}");
                TempData["Error"] = "Error al agregar al carrito";
                return RedirectToPage();
            }
        }
    }
}