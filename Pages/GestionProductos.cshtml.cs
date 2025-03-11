using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Services;

namespace Pages
{
    [Authorize]
    public class GestionProductosModel : PageModel
    {
        private readonly ILogger<GestionProductosModel> _logger;
        private static List<CarritoItem> _itemsCarrito = new();
        private static List<Producto> _productos = new()
        {
            new Producto { Id = 1, Nombre = "Producto 1", TipoOferta = "Normal", Stock = 10, Precio = 100 },
            new Producto { Id = 2, Nombre = "Producto 2", TipoOferta = "Oferta", Stock = 15, Precio = 200 },
            new Producto { Id = 3, Nombre = "Producto 3", TipoOferta = "Normal", Stock = 20, Precio = 150 }
        };

        private static Dictionary<int, int> _seleccionados = new();

        public List<Producto> Productos => _productos;
        public Dictionary<int, int> Seleccionados => _seleccionados;

        public GestionProductosModel(ILogger<GestionProductosModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnPostAgregarAlCarrito(int productoId, string nombre, decimal precio, string tipoOferta, string imagenUrl, int cantidad)
        {
            try
            {
                var itemExistente = _itemsCarrito.FirstOrDefault(i => i.Id == productoId);
                if (itemExistente != null)
                {
                    itemExistente.Cantidad += cantidad;
                }
                else
                {
                    _itemsCarrito.Add(new CarritoItem
                    {
                        Id = productoId,
                        Nombre = nombre,
                        Precio = precio,
                        Cantidad = cantidad,
                        TipoOferta = tipoOferta,
                        ImagenUrl = imagenUrl
                    });
                }

                TempData["Mensaje"] = $"Se agregó {cantidad} {nombre}(s) al carrito";
                return RedirectToPage("/Carrito");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al agregar al carrito: {ex.Message}");
                TempData["Error"] = "Error al agregar al carrito";
                return RedirectToPage();
            }
        }

        public IActionResult OnPost(Dictionary<int, int> cantidades)
        {
            foreach (var cantidad in cantidades)
            {
                if (cantidad.Value > 0)
                {
                    if (_seleccionados.ContainsKey(cantidad.Key))
                    {
                        _seleccionados[cantidad.Key] += cantidad.Value;
                    }
                    else
                    {
                        _seleccionados[cantidad.Key] = cantidad.Value;
                    }
                }
            }
            return RedirectToPage("/Carrito");
        }
    }

    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string TipoOferta { get; set; } = "";
        public int Stock { get; set; }
        public decimal Precio { get; set; }
    }
} 