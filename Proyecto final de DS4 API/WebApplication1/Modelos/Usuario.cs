using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Proyecto_API_DS4.Modelos
{
    public class Usuario
    {
        [Key]
        [JsonPropertyName("usuarioId")]
        public int usuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("nombreUsuario")]
        public string nombreUsuario { get; set; } = string.Empty;

        [MaxLength(200)]
        [JsonPropertyName("direccion")]
        public string? direccion { get; set; }

        [JsonPropertyName("celular")]
        public string? celular { get; set; }

        [Required]
        [EmailAddress]
        [JsonPropertyName("correo")]
        public string correo { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("contrasena")]
        public string contrasena { get; set; } = string.Empty;
    }
}
