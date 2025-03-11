namespace miAplicacion.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string TipoOferta { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public decimal Total => Precio * Cantidad;
    }
}