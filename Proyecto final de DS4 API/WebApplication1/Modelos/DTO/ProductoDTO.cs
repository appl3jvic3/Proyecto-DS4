namespace Proyecto_API_DS4.Modelos.DTO
{
    public class ProductoDTO
    {
        public int productoId { get; set; }
        public string nombreProducto { get; set; } = string.Empty;
        public string? descripcion { get; set; }
        public decimal precio { get; set; }
        public int cantidadDisponible { get; set; }
        public string categoria { get; set; } = string.Empty;
        public decimal descuento { get; set; }
    }
}
