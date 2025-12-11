using System.Text.Json.Serialization;

namespace Proyecto_API_DS4.Modelos.DTO
{
    public class OrderItemDto
    {
        [JsonPropertyName("orderId")]
        public int numeroCompra { get; set; }

        [JsonPropertyName("productId")]
        public int productoId { get; set; }

        [JsonPropertyName("productName")]
        public string nombreProducto { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int cantidad { get; set; }

        [JsonPropertyName("unitPrice")]
        public decimal precioUnitario { get; set; }

        [JsonPropertyName("total")]
        public decimal totalItem { get; set; }

        [JsonPropertyName("date")]
        public DateTime fechaCompra { get; set; }
    }
}
