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

        if (_carrito.TryGetValue(id, out int cantidad))
        {
            var producto = _productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                producto.Stock += cantidad; // Devolver al stock
                _carrito.Remove(id);

                // Guardar los cambios
                GuardarProductos();
                GuardarCarrito();

                TempData["Mensaje"] = "Producto eliminado del carrito.";
            }
        }

        return RedirectToPage();
    }

    public JsonResult OnPostActualizarCantidad(int id, int cantidad)
    {
        CargarDatos();
        var producto = _productos.FirstOrDefault(p => p.Id == id);

        if (producto == null)
            return new JsonResult(new { success = false, message = "Producto no encontrado" });

        if (cantidad < 0)
            return new JsonResult(new { success = false, message = "La cantidad no puede ser negativa" });

        // Si hay cantidad actual en el carrito, calculamos la diferencia
        if (_carrito.TryGetValue(id, out int cantidadActual))
        {
            int diferencia = cantidad - cantidadActual;

            // Si estamos aumentando la cantidad
            if (diferencia > 0)
            {
                if (diferencia > producto.Stock)
                    return new JsonResult(new { success = false, message = "No hay suficiente stock" });

                producto.Stock -= diferencia;
            }
            // Si estamos disminuyendo la cantidad
            else if (diferencia < 0)
            {
                producto.Stock += Math.Abs(diferencia);
            }

            if (cantidad == 0)
                _carrito.Remove(id);
            else
                _carrito[id] = cantidad;
        }

        GuardarProductos();
        GuardarCarrito();

        return new JsonResult(new { success = true });
    }

    private void CargarDatos()
    {
        var productosJson = HttpContext.Session.GetString(PRODUCTOS_KEY);
        if (!string.IsNullOrEmpty(productosJson))
        {
            _productos = JsonSerializer.Deserialize<List<Producto>>(productosJson) ?? new();
        }

        var carritoJson = HttpContext.Session.GetString(CARRITO_KEY);
        if (!string.IsNullOrEmpty(carritoJson))
        {
            _carrito = JsonSerializer.Deserialize<Dictionary<int, int>>(carritoJson) ?? new();
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

    private void GuardarProductos()
    {
        var productosJson = JsonSerializer.Serialize(_productos);
        HttpContext.Session.SetString(PRODUCTOS_KEY, productosJson);
    }
}