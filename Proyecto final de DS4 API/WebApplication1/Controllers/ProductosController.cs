using Microsoft.AspNetCore.Mvc;
using Proyecto_API_DS4.Data;
using Proyecto_API_DS4.Modelos;
using Proyecto_API_DS4.Modelos.DTO;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_API_DS4.Controllers
{
    [ApiController]
        [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }
        //GET: api/Productos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _context.Producto
                .AsNoTracking()
                .ToListAsync();

            // Generar rutas de imágenes para cada producto
            foreach (var p in productos)
            {
                var nombre = p.nombreProducto.ToLower()
                    .Replace(" ", "-")
                    .Replace("á", "a").Replace("é", "e")
                    .Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
                p.imagen = $"img/productos/{nombre}.png";
            }

            return Ok(productos);
        }

        [HttpGet("{productoId:int}")]
        public async Task<IActionResult> Get(int productoId)
        {
            var p = await _context.Producto.FindAsync(productoId);
            if (p == null) return NotFound();

            // Generar ruta de imagen
            var nombre = p.nombreProducto.ToLower()
                .Replace(" ", "-")
                .Replace("á", "a").Replace("é", "e")
                .Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
            p.imagen = $"img/productos/{nombre}.png";

            return Ok(p);
        }


        // PUT: api/productos/{productoId}
        [HttpPut("{productoId:int}")]
        public async Task<IActionResult> Update(int productoId, [FromBody] ProductoDTO productoDto)
        {
            if (productoId != productoDto.productoId) return BadRequest("productoId mismatch");

            var producto = await _context.Set<Producto>().FindAsync(productoId);
            if (producto == null) return NotFound();

            producto.nombreProducto = productoDto.nombreProducto;
            producto.descripcion = productoDto.descripcion ?? string.Empty;
            producto.precio = productoDto.precio;
            producto.cantidadDisponible = productoDto.cantidadDisponible;
            producto.categoria = productoDto.categoria;
            producto.descuento = productoDto.descuento;

            _context.Set<Producto>().Update(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       

    
    }
}
