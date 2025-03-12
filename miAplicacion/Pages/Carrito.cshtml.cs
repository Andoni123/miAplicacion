using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Linq; // A�adir esta l�nea
using System.Collections.Generic; // A�adir esta l�nea
using miAplicacion.Models; // A�adir esta l�nea

namespace miAplicacion.Pages;

public class CarritoModel : PageModel
{
    private const string PRODUCTOS_KEY = "productos";
    private const string CARRITO_KEY = "carrito";
    
    private List<Producto> _productos = new();
    private Dictionary<int, int> _carrito = new();

    public List<(Producto Producto, int Cantidad)> ItemsCarrito { get; private set; } = new();
    public decimal Total => ItemsCarrito.Sum(item => item.Producto.Precio * item.Cantidad);

    public void OnGet()
    {
        CargarDatos();
        ActualizarItemsCarrito();
    }

    public IActionResult OnPostEliminarDelCarrito(int id)
    {
        CargarDatos();

        if (_carrito.ContainsKey(id))
        {
            _carrito.Remove(id);
            GuardarCarrito();
            TempData["Mensaje"] = "Producto eliminado del carrito.";
        }

        return RedirectToPage();
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

    private void ActualizarItemsCarrito()
    {
        ItemsCarrito = _carrito
            .Select(c => (_productos.FirstOrDefault(p => p.Id == c.Key), c.Value))
            .Where(item => item.Item1 != null)
            .ToList();
    }

    private void GuardarCarrito()
    {
        var carritoJson = JsonSerializer.Serialize(_carrito);
        HttpContext.Session.SetString(CARRITO_KEY, carritoJson);
    }
}