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
        public decimal Total { get; set; }

        public void OnGet()
        {
            // Simulamos items en el carrito
            ItemsCarrito = new List<CarritoItem>
            {
                new CarritoItem 
                {
                    Id = 1,
                    Nombre = "MacBook Pro 2023",
                    Precio = 1299.99M,
                    Cantidad = 1,
                    ImagenUrl = "/images/macbook.jpg"
                },
                new CarritoItem 
                {
                    Id = 2,
                    Nombre = "iPhone 14 Pro",
                    Precio = 999.99M,
                    Cantidad = 2,
                    ImagenUrl = "/images/iphone.jpg"
                },
                new CarritoItem 
                {
                    Id = 3,
                    Nombre = "AirPods Pro",
                    Precio = 249.99M,
                    Cantidad = 1,
                    ImagenUrl = "/images/airpods.jpg"
                }
            };

            // Calcular el total
            Total = ItemsCarrito.Sum(item => item.Precio * item.Cantidad);
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

    public class CarritoItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string ImagenUrl { get; set; }
        public string TipoOferta { get; set; }
    }
} 