using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Proyecto_API_DS4.Modelos
{
    public class Producto
    {
        [JsonPropertyName("id")]
        public int productoId { get; set; }

        [JsonPropertyName("name")]
        public string nombreProducto { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string descripcion { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal precio { get; set; }

        [JsonPropertyName("stock")]
        public int cantidadDisponible { get; set; }

        [JsonPropertyName("category")]
        public string categoria { get; set; } = string.Empty;

        [JsonPropertyName("discount")]
        public decimal descuento { get; set; }


        [NotMapped]
        [JsonPropertyName("image")]
        public string imagen { get; set; } = string.Empty;
    }
}
