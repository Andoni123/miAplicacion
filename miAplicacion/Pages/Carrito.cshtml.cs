using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Linq; // Añadir esta línea
using System.Collections.Generic; // Añadir esta línea
using miAplicacion.Models; // Añadir esta línea

namespace miAplicacion.Pages;

public class CarritoModel : PageModel
{
    public List<(Producto Producto, int Cantidad)> ItemsCarrito
    {
        get
        {
            var productos = GestionProductosModel._productos;
            return GestionProductosModel._seleccionados
                .Select(s => (productos.FirstOrDefault(p => p.Id == s.Key), s.Value))
                .Where(item => item.Item1 != null)
                .ToList();
        }
    }

    public decimal Total => ItemsCarrito.Sum(item => item.Producto.Precio * item.Cantidad);

    private void GuardarCarrito()
    {
        var carritoJson = JsonSerializer.Serialize(GestionProductosModel._seleccionados);
        HttpContext.Session.SetString("carrito", carritoJson);
    }

    public IActionResult OnPostEliminarDelCarrito(int id)
    {
        if (GestionProductosModel._seleccionados.ContainsKey(id))
        {
            var cantidad = GestionProductosModel._seleccionados[id];
            var producto = GestionProductosModel._productos.FirstOrDefault(p => p.Id == id);
            
            if (producto != null)
            {
                producto.Stock += cantidad; // Devolver al stock
                GestionProductosModel._seleccionados.Remove(id);
                
                // Guardar cambios
                var productosJson = JsonSerializer.Serialize(GestionProductosModel._productos);
                HttpContext.Session.SetString("productos", productosJson);
                GuardarCarrito();

                TempData["Mensaje"] = "Producto eliminado del carrito.";
            }
        }

        return RedirectToPage();
    }
}