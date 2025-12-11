using Proyecto_API_DS4.Modelos.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_API_DS4.Data;
using Proyecto_API_DS4.Modelos;

namespace Proyecto_API_DS4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            // Normalizar el correo (quitar espacios y convertir a minusculas)
            var correoNormalizado = request.correo?.Trim().ToLower();

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.correo.ToLower() == correoNormalizado);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Usuario no encontrado" });
            }

            // Comparar contrasena (quitando espacios)
            if (usuario.contrasena.Trim() != request.contrasena.Trim())
            {
                return Unauthorized(new { message = "Contraseña incorrecta" });
            }

           
            // El frontend busca user.nombreUsuario y user.correo en localStorage
            return Ok(new
            {
                token = GenerarToken(usuario),
                user = new
                {
                    usuarioId = usuario.usuarioId,
                    nombreUsuario = usuario.nombreUsuario, 
                    correo = usuario.correo                 
                }
            });
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            // Validar que los campos requeridos no sean null o vacios
            if (string.IsNullOrWhiteSpace(request.correo))
            {
                return BadRequest(new { message = "El correo es requerido" });
            }

            if (string.IsNullOrWhiteSpace(request.nombreUsuario))
            {
                return BadRequest(new { message = "El nombre de usuario es requerido" });
            }

            if (string.IsNullOrWhiteSpace(request.contrasena))
            {
                return BadRequest(new { message = "La contraseña es requerida" });
            }

            var correoNormalizado = request.correo.Trim().ToLower();

            // Verificar si ya existe
            if (await _context.Usuario.AnyAsync(u => u.correo.ToLower() == correoNormalizado))
            {
                return BadRequest(new { message = "El correo ya está registrado" });
            }

            var nuevoUsuario = new Usuario
            {
                nombreUsuario = request.nombreUsuario.Trim(),
                correo = correoNormalizado,
                contrasena = request.contrasena.Trim(),
                celular = null
            };

            _context.Usuario.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            
            // Mantiene consistencia con el formato usado en el metodo Login
            return Ok(new
            {
                message = "Usuario registrado exitosamente",
                user = new
                {
                    usuarioId = nuevoUsuario.usuarioId,
                    nombreUsuario = nuevoUsuario.nombreUsuario,  // Antes era: name
                    correo = nuevoUsuario.correo                 // Antes era: email
                }
            });
        }

        
        [HttpGet("user/{usuarioId:int}")]
        public async Task<IActionResult> GetUsuario(int usuarioId)
        {
            try
            {
                // Buscar usuario por ID y seleccionar solo los campos necesarios
                // No incluimos la contrasena por seguridad
                var usuario = await _context.Usuario
                    .Where(u => u.usuarioId == usuarioId)
                    .Select(u => new
                    {
                        usuarioId = u.usuarioId,
                        nombreUsuario = u.nombreUsuario,
                        correo = u.correo
                    })
                    .FirstOrDefaultAsync();

                // Validar si el usuario existe
                if (usuario == null)
                {
                    return NotFound(new { mensaje = $"Usuario con ID {usuarioId} no encontrado" });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                return StatusCode(500, new { mensaje = "Error al obtener usuario", error = ex.Message });
            }
        }

        private string GenerarToken(Usuario usuario)
        {
            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{usuario.usuarioId}:{usuario.correo}:{DateTime.Now}")
            );
        }
    }
}