using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using miAplicacion.Models;
using miAplicacion.Extensions;

namespace miAplicacion.Pages
{
    public class CarritoModel : PageModel
    {
        public List<CarritoItem> ItemsCarrito { get; set; }
        public decimal Total => ItemsCarrito?.Sum(i => i.Total) ?? 0;

        public void OnGet()
        {
            ItemsCarrito = HttpContext.Session.GetObject<List<CarritoItem>>("Carrito") ?? new List<CarritoItem>();
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
            return RedirectToPage();
        }
    }
} 