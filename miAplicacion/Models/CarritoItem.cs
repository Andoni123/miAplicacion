namespace miAplicacion.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string TipoOferta { get; set; }
        public decimal Total => Cantidad * Precio;
    }
} 