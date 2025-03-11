using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using miAplicacion.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace miAplicacion.Pages
{
    [Authorize]
    public class CarritoModel : PageModel
    {
        private readonly ILogger<CarritoModel> _logger;
        private static List<Producto> _productos = new()
        {
            new() { Id = 1, Nombre = "Producto 1", TipoOferta = "Normal", Stock = 10, Precio = 100 },
            new() { Id = 2, Nombre = "Producto 2", TipoOferta = "Oferta", Stock = 15, Precio = 200 },
        };
        private static Dictionary<int, int> _seleccionados = new();

        public CarritoModel(ILogger<CarritoModel> logger)
        {
            _logger = logger;
        }

        public List<(Producto Producto, int Cantidad)> ItemsCarrito
        {
            get
            {
                var productos = GestionProductosModel._productos;
                return GestionProductosModel._seleccionados
                    .Select(s => (productos.First(p => p.Id == s.Key), s.Value))
                    .ToList();
            }
        }

        public decimal Total => ItemsCarrito.Sum(item => item.Producto.Precio * item.Cantidad);

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostEliminar(int id)
        {
            try
            {
                var item = _seleccionados.FirstOrDefault(i => i.Key == id);
                if (item.Key != 0)
                {
                    _seleccionados.Remove(item.Key);
                    TempData["Mensaje"] = "Producto eliminado del carrito";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar producto: {ex.Message}");
                TempData["Error"] = "Error al eliminar producto";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostActualizarCantidad(int id, int cantidad)
        {
            try
            {
                var item = _seleccionados.FirstOrDefault(i => i.Key == id);
                if (item.Key != 0)
                {
                    if (cantidad > 0)
                    {
                        _seleccionados[item.Key] = cantidad;
                    }
                    else
                    {
                        _seleccionados.Remove(item.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar cantidad: {ex.Message}");
                TempData["Error"] = "Error al actualizar cantidad";
            }
            return RedirectToPage();
        }
    }
} 