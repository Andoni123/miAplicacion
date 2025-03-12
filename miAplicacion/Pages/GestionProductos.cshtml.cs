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
    
    private const string PRODUCTOS_KEY = "productos";
    private const string CARRITO_KEY = "carrito";
    
    private List<Producto> _productos = new();
    private Dictionary<int, int> _carrito = new();

    public List<Producto> Productos => _productos;
    
    public void OnGet()
    {
        // Recuperar productos del SessionStorage
        var productosJson = HttpContext.Session.GetString(PRODUCTOS_KEY);
        if (string.IsNullOrEmpty(productosJson))
        {
            // Inicializar con productos por defecto si no hay datos
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
            GuardarProductos();
        }
        else
        {
            _productos = JsonSerializer.Deserialize<List<Producto>>(productosJson);
        }

        // Recuperar carrito
        var carritoJson = HttpContext.Session.GetString(CARRITO_KEY);
        if (!string.IsNullOrEmpty(carritoJson))
        {
            _carrito = JsonSerializer.Deserialize<Dictionary<int, int>>(carritoJson);
        }
    }

    public IActionResult OnPostCrearProducto()
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Por favor, complete todos los campos correctamente.";
            return RedirectToPage();
        }

        // Recuperar productos actuales
        var productosJson = HttpContext.Session.GetString(PRODUCTOS_KEY);
        if (!string.IsNullOrEmpty(productosJson))
        {
            _productos = JsonSerializer.Deserialize<List<Producto>>(productosJson);
        }

        NuevoProducto.Id = _productos.Any() ? _productos.Max(p => p.Id) + 1 : 1;
        _productos.Add(NuevoProducto);
        
        GuardarProductos();
        TempData["Mensaje"] = $"Producto '{NuevoProducto.Nombre}' creado exitosamente.";
        return RedirectToPage();
    }

    public IActionResult OnPostEliminarProducto(int id)
    {
        // Recuperar datos actuales
        CargarDatos();

        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto != null)
        {
            _productos.Remove(producto);
            
            // Eliminar del carrito si existe
            if (_carrito.ContainsKey(id))
            {
                _carrito.Remove(id);
                GuardarCarrito();
            }

            GuardarProductos();
            TempData["Mensaje"] = $"Producto '{producto.Nombre}' eliminado exitosamente.";
        }

        return RedirectToPage();
    }

    public IActionResult OnPost(Dictionary<int, int> cantidades)
    {
        if (cantidades == null) return RedirectToPage();
        
        // Recuperar datos actuales
        CargarDatos();

        foreach (var cantidad in cantidades)
        {
            if (cantidad.Value > 0)
            {
                var producto = _productos.FirstOrDefault(p => p.Id == cantidad.Key);
                if (producto != null)
                {
                    int cantidadActualEnCarrito = _carrito.ContainsKey(cantidad.Key) ? _carrito[cantidad.Key] : 0;
                    int cantidadTotal = cantidadActualEnCarrito + cantidad.Value;
                    
                    if (cantidadTotal <= producto.Stock)
                    {
                        if (_carrito.ContainsKey(cantidad.Key))
                            _carrito[cantidad.Key] += cantidad.Value;
                        else
                            _carrito[cantidad.Key] = cantidad.Value;
                    }
                    else
                    {
                        TempData["Error"] = $"No hay suficiente stock para '{producto.Nombre}'";
                        return RedirectToPage();
                    }
                }
            }
        }

        GuardarCarrito();
        return RedirectToPage("/Carrito");
    }

    private void CargarDatos()
    {
        var productosJson = HttpContext.Session.GetString(PRODUCTOS_KEY);
        if (!string.IsNullOrEmpty(productosJson))
        {
            _productos = JsonSerializer.Deserialize<List<Producto>>(productosJson);
        }

        var carritoJson = HttpContext.Session.GetString(CARRITO_KEY);
        if (!string.IsNullOrEmpty(carritoJson))
        {
            _carrito = JsonSerializer.Deserialize<Dictionary<int, int>>(carritoJson);
        }
    }

    private void GuardarProductos()
    {
        var productosJson = JsonSerializer.Serialize(_productos);
        HttpContext.Session.SetString(PRODUCTOS_KEY, productosJson);
    }

    private void GuardarCarrito()
    {
        var carritoJson = JsonSerializer.Serialize(_carrito);
        HttpContext.Session.SetString(CARRITO_KEY, carritoJson);
    }
}
