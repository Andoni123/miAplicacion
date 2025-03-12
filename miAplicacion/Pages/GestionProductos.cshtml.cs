using miAplicacion.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace miAplicacion.Pages;

public class GestionProductosModel : PageModel
{
    private const string PRODUCTOS_KEY = "productos";
    private const string CARRITO_KEY = "carrito";

    [BindProperty]
    public Producto NuevoProducto { get; set; } = new();

    private readonly List<Producto> _productos = new();
    private readonly Dictionary<int, int> _seleccionados = new();

    public IEnumerable<Producto> Productos => _productos.Where(p => p.Stock > 0);

    public void OnGet()
    {
        var productosJson = HttpContext.Session.GetString(PRODUCTOS_KEY);
        if (!string.IsNullOrEmpty(productosJson))
        {
            _productos.Clear();
            _productos.AddRange(JsonSerializer.Deserialize<List<Producto>>(productosJson) ?? new List<Producto>());
        }
        else if (_productos.Count == 0)
        {
            _productos.AddRange(new List<Producto>
            {
                new() { Id = 1, Nombre = "Camiseta Nike Dri-FIT", TipoOferta = "Normal", Stock = 25, Precio = 29.99m },
                new() { Id = 2, Nombre = "Zapatillas Adidas Running", TipoOferta = "Oferta", Stock = 15, Precio = 89.99m },
                new() { Id = 3, Nombre = "Pantalón Under Armour", TipoOferta = "Normal", Stock = 20, Precio = 49.99m },
                new() { Id = 4, Nombre = "Sudadera Puma Essential", TipoOferta = "Oferta", Stock = 12, Precio = 39.99m },
                new() { Id = 5, Nombre = "Calcetines Nike (Pack 3)", TipoOferta = "Normal", Stock = 30, Precio = 14.99m },
                new() { Id = 6, Nombre = "Shorts Nike Flex", TipoOferta = "Normal", Stock = 18, Precio = 24.99m },
                new() { Id = 7, Nombre = "Mochila Adidas Classic", TipoOferta = "Oferta", Stock = 10, Precio = 34.99m },
                new() { Id = 8, Nombre = "Balón de Fútbol Nike Strike", TipoOferta = "Normal", Stock = 22, Precio = 19.99m },
                new() { Id = 9, Nombre = "Botella de Agua Nike", TipoOferta = "Normal", Stock = 35, Precio = 12.99m },
                new() { Id = 10, Nombre = "Gorra Adidas Baseball", TipoOferta = "Normal", Stock = 28, Precio = 17.99m },
                new() { Id = 11, Nombre = "Banda Elástica Fitness", TipoOferta = "Oferta", Stock = 40, Precio = 9.99m },
                new() { Id = 12, Nombre = "Guantes de Gimnasio", TipoOferta = "Normal", Stock = 15, Precio = 15.99m },
                new() { Id = 13, Nombre = "Esterilla Yoga Premium", TipoOferta = "Oferta", Stock = 20, Precio = 29.99m },
                new() { Id = 14, Nombre = "Rodilleras Deportivas", TipoOferta = "Normal", Stock = 25, Precio = 19.99m },
                new() { Id = 15, Nombre = "Bolsa Deporte Puma", TipoOferta = "Normal", Stock = 17, Precio = 44.99m }
            });
            GuardarProductos();
        }

        var carritoJson = HttpContext.Session.GetString(CARRITO_KEY);
        if (!string.IsNullOrEmpty(carritoJson))
        {
            _seleccionados.Clear();
            foreach (var item in JsonSerializer.Deserialize<Dictionary<int, int>>(carritoJson) ?? new Dictionary<int, int>())
            {
                _seleccionados[item.Key] = item.Value;
            }
        }
    }

    public IActionResult OnPostEliminarProducto(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto != null)
        {
            _productos.Remove(producto);
            GuardarProductos();

            if (_seleccionados.Remove(id))
            {
                GuardarCarrito();
            }

            TempData["Mensaje"] = $"Producto '{producto.Nombre}' eliminado exitosamente.";
        }

        return RedirectToPage();
    }

    public IActionResult OnPost(Dictionary<int, int> cantidades)
    {
        if (cantidades?.Count == 0) return RedirectToPage();

        CargarDatos();

        foreach (var (id, cantidad) in cantidades)
        {
            if (cantidad <= 0) continue;

            var producto = _productos.FirstOrDefault(p => p.Id == id);
            if (producto == null) continue;

            if (_seleccionados.TryGetValue(id, out int cantidadActual))
            {
                int cantidadTotal = cantidadActual + cantidad;
                if (cantidadTotal <= producto.Stock)
                {
                    _seleccionados[id] = cantidadTotal;
                    producto.Stock -= cantidad;
                }
            }
            else if (cantidad <= producto.Stock)
            {
                _seleccionados[id] = cantidad;
                producto.Stock -= cantidad;
            }
        }

        GuardarProductos();
        GuardarCarrito();
        return RedirectToPage("/Carrito");
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

        GuardarProductos();
        TempData["Mensaje"] = $"Producto '{NuevoProducto.Nombre}' creado exitosamente.";
        return RedirectToPage();
    }

    private void GuardarProductos()
    {
        var productosJson = JsonSerializer.Serialize(_productos);
        HttpContext.Session.SetString(PRODUCTOS_KEY, productosJson);
    }

    private void GuardarCarrito()
    {
        var carritoJson = JsonSerializer.Serialize(_seleccionados);
        HttpContext.Session.SetString(CARRITO_KEY, carritoJson);
    }

    private void CargarDatos()
    {
        var productosJson = HttpContext.Session.GetString(PRODUCTOS_KEY);
        if (!string.IsNullOrEmpty(productosJson))
        {
            _productos.Clear();
            _productos.AddRange(JsonSerializer.Deserialize<List<Producto>>(productosJson) ?? new());
        }

        var carritoJson = HttpContext.Session.GetString(CARRITO_KEY);
        if (!string.IsNullOrEmpty(carritoJson))
        {
            _seleccionados.Clear();
            foreach (var item in JsonSerializer.Deserialize<Dictionary<int, int>>(carritoJson) ?? new())
            {
                _seleccionados[item.Key] = item.Value;
            }
        }
    }
}
