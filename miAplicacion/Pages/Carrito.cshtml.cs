using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using miAplicacion.Models;
using miAplicacion.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace miAplicacion.Pages
{
    [Authorize]
    public class CarritoModel : PageModel
    {
        private readonly ILogger<CarritoModel> _logger;

        public CarritoModel(ILogger<CarritoModel> logger)
        {
            _logger = logger;
        }

        public List<CarritoItem> ItemsCarrito { get; set; } = new List<CarritoItem>();
        public decimal Total { get; set; }

        public void OnGet()
        {
            try
            {
                ItemsCarrito = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();
                _logger.LogInformation($"Carrito recuperado con {ItemsCarrito.Count} items");
                CalcularTotal();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al cargar el carrito: {ex.Message}");
                ItemsCarrito = new List<CarritoItem>();
            }
        }

        public IActionResult OnPostAgregarAlCarrito(int productoId, string nombre, decimal precio, int cantidad, string tipoOferta, string imagenUrl)
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
                ItemsCarrito = carritoItems; // Actualizar la propiedad ItemsCarrito
                CalcularTotal(); // Recalcular el total
                TempData["MensajeExito"] = "Producto agregado al carrito exitosamente";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al agregar al carrito: {ex.Message}");
                TempData["MensajeError"] = "Error al agregar al carrito: " + ex.Message;
                return RedirectToPage();
            }
        }

        public IActionResult OnPostEliminar(int id)
        {
            var carritoItems = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();
            var item = carritoItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                carritoItems.Remove(item);
                HttpContext.Session.SetObject("Carrito", carritoItems);
            }
            ModelState.Clear(); // Clear the model state
            return RedirectToPage();
        }

        public IActionResult OnPostActualizarCantidad(int id, int cantidad)
        {
            var carritoItems = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();
            var item = carritoItems.FirstOrDefault(i => i.Id == id);
            
            if (item != null && cantidad >= 0)
            {
                item.Cantidad = cantidad;
                if (cantidad == 0)
                {
                    carritoItems.Remove(item);
                }
                HttpContext.Session.SetObject("Carrito", carritoItems);
            }
            
            return RedirectToPage();
        }

        public JsonResult OnGetObtenerTotal()
        {
            var carritoItems = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();
            var total = carritoItems.Sum(item => item.Precio * item.Cantidad);
            return new JsonResult(new { total = total.ToString("N2") });
        }

        private void CalcularTotal()
        {
            Total = ItemsCarrito.Sum(item => item.Precio * item.Cantidad);
        }
    }
}