using System.Text.Json.Serialization;

namespace Proyecto_API_DS4.Modelos.DTO
{
    public class CarritoDTO
    {
     

        [JsonPropertyName("id")]
        public int? numeroCompra { get; set; }

        [JsonPropertyName("usuarioId")] 
        public int usuarioId { get; set; }

        [JsonPropertyName("productoId")]  
        public int productoId { get; set; }

        [JsonPropertyName("cantidad")]  
        public int cantidad { get; set; }

        [JsonPropertyName("precioTotal")] 
        public decimal precioTotal { get; set; }
    }
}