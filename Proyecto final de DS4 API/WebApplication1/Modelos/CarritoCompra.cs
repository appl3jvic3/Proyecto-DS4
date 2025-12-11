using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Proyecto_API_DS4.Modelos
{
    public class CarritoCompra
    {
        [Key]
        [JsonPropertyName("id")]
        public int numeroCompra { get; set; }

        [JsonPropertyName("userId")]
        public int usuarioId { get; set; }

        [JsonPropertyName("productId")]
        public int productoId { get; set; }

        [JsonPropertyName("quantity")]
        public int cantidad { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal precioTotal { get; set; }

        [ForeignKey("usuarioId")]
        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        [ForeignKey("productoId")]
        [JsonIgnore]
        public Producto? Producto { get; set; }

        [JsonPropertyName("date")]
        public DateTime? fechaCompra { get; set; }
    }
}
