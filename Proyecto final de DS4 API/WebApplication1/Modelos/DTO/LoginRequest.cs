using System.Text.Json.Serialization;

namespace Proyecto_API_DS4.Modelos.DTO
{
    public class LoginRequest
    {
        [JsonPropertyName("correo")]
        public string correo { get; set; }

        [JsonPropertyName("contrasena")]
        public string contrasena { get; set; }
    }
}
