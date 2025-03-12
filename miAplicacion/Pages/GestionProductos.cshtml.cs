// FILE CONTEXT
using miAplicacion.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace miAplicacion.Pages;

public class GestionProductosModel : PageModel
{
    [BindProperty]
    public Producto NuevoProducto { get; set; } = new();

    public static List<Producto> _productos = new();
    public static Dictionary<int, int> _seleccionados = new();

    public List<Producto> Productos => _productos;

    public void OnGet()
    {
        if (!_productos.Any()) // Solo inicializar si no hay productos
        {
            _productos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Camiseta Nike Dri-FIT", TipoOferta = "Normal", Stock = 25, Precio = 29.99m },
                new Producto { Id = 2, Nombre = "Zapatillas Adidas Running", TipoOferta = "Oferta", Stock = 15, Precio = 89.99m },
                new Producto { Id = 3, Nombre = "Pantalón Under Armour", TipoOferta = "Normal", Stock = 20, Precio = 49.99m },
                new Producto { Id = 4, Nombre = "Sudadera Puma Essential", TipoOferta = "Oferta", Stock = 12, Precio = 39.99m },
                new Producto { Id = 5, Nombre = "Calcetines Nike (Pack 3)", TipoOferta = "Normal", Stock = 30, Precio = 14.99m },
                new Producto { Id = 6, Nombre = "Shorts Nike Flex", TipoOferta = "Normal", Stock = 18, Precio = 24.99m },
                new Producto { Id = 7, Nombre = "Mochila Adidas Classic", TipoOferta = "Oferta", Stock = 10, Precio = 34.99m },
                new Producto { Id = 8, Nombre = "Balón de Fútbol Nike Strike", TipoOferta = "Normal", Stock = 22, Precio = 19.99m },
                new Producto { Id = 9, Nombre = "Botella de Agua Nike", TipoOferta = "Normal", Stock = 35, Precio = 12.99m },
                new Producto { Id = 10, Nombre = "Gorra Adidas Baseball", TipoOferta = "Normal", Stock = 28, Precio = 17.99m },
                new Producto { Id = 11, Nombre = "Banda Elástica Fitness", TipoOferta = "Oferta", Stock = 40, Precio = 9.99m },
                new Producto { Id = 12, Nombre = "Guantes de Gimnasio", TipoOferta = "Normal", Stock = 15, Precio = 15.99m },
                new Producto { Id = 13, Nombre = "Esterilla Yoga Premium", TipoOferta = "Oferta", Stock = 20, Precio = 29.99m },
                new Producto { Id = 14, Nombre = "Rodilleras Deportivas", TipoOferta = "Normal", Stock = 25, Precio = 19.99m },
                new Producto { Id = 15, Nombre = "Bolsa Deporte Puma", TipoOferta = "Normal", Stock = 17, Precio = 44.99m }
            };
        }
    }

    public IActionResult OnPostCrearProducto()
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Por favor, complete todos los campos correctamente.";
            return RedirectToPage();
        }

        NuevoProducto.Id = _productos.Any() ? _productos.Max(p => p.Id) + 1 : 1;
        _productos.Add(NuevoProducto);
        
        TempData["Mensaje"] = $"Producto '{NuevoProducto.Nombre}' creado exitosamente.";
        return RedirectToPage();
    }

    public IActionResult OnPostEliminarProducto(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto != null)
        {
            _productos.Remove(producto);

            // Eliminar del carrito si existe
            if (_seleccionados.ContainsKey(id))
            {
                _seleccionados.Remove(id);
            }

            TempData["Mensaje"] = $"Producto '{producto.Nombre}' eliminado exitosamente.";
        }

        return RedirectToPage();
    }

    public IActionResult OnPost(Dictionary<int, int> cantidades)
    {
        if (cantidades == null)
        {
            return RedirectToPage();
        }

        foreach (var cantidad in cantidades)
        {
            if (cantidad.Value > 0)
            {
                var producto = _productos.FirstOrDefault(p => p.Id == cantidad.Key);
                if (producto != null && cantidad.Value <= producto.Stock)
                {
                    // Verificar si hay suficiente stock
                    int cantidadActualEnCarrito = _seleccionados.ContainsKey(cantidad.Key) ? _seleccionados[cantidad.Key] : 0;
                    int cantidadTotal = cantidadActualEnCarrito + cantidad.Value;
                    
                    if (cantidadTotal <= producto.Stock)
                    {
                        if (_seleccionados.ContainsKey(cantidad.Key))
                            _seleccionados[cantidad.Key] += cantidad.Value;
                        else
                            _seleccionados[cantidad.Key] = cantidad.Value;
                            
                        // No reducimos el stock aquí, solo cuando se confirma la compra
                    }
                    else
                    {
                        TempData["Error"] = $"No hay suficiente stock para el producto '{producto.Nombre}'";
                        return RedirectToPage();
                    }
                }
            }
        }

        return RedirectToPage("/Carrito");
    }
}
