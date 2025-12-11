using Proyecto_API_DS4.Data;
using Proyecto_API_DS4.Modelos.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Proyecto_API_DS4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistorialController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HistorialController> _logger;

        public HistorialController(IConfiguration configuration, ILogger<HistorialController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // GET api/Historial/user/1?page=1&pageSize=10&from=2025-01-01&to=2025-12-31
        [HttpGet("user/{usuarioId:int}")]
        public async Task<IActionResult> GetOrdersByUser(int usuarioId, int page = 1, int pageSize = 10, DateTime? from = null, DateTime? to = null)
        {
            try
            {
                var lista = new List<OrderDto>();
                var connString = _configuration.GetConnectionString("DefaultConnection");

                using (var conn = new SqlConnection(connString))
                using (var cmd = new SqlCommand("usp_GetOrdersByUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", usuarioId);
                    cmd.Parameters.AddWithValue("@Page", page);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@FromDate", (object?)from ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate", (object?)to ?? DBNull.Value);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new OrderDto
                            {
                                numeroCompra = reader.GetInt32(reader.GetOrdinal("numeroCompra")),
                                usuarioId = reader.GetInt32(reader.GetOrdinal("usuarioId")),
                                fechaCompra = reader.GetDateTime(reader.GetOrdinal("fechaCompra")),
                                totalAmount = reader.GetDecimal(reader.GetOrdinal("totalAmount")),
                                itemsCount = reader.GetInt32(reader.GetOrdinal("itemsCount"))
                            });
                        }
                    }
                }

                return Ok(lista);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al obtener historial usuario {UserId}", usuarioId);
                return StatusCode(500, new { message = "Error al obtener historial" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener historial usuario {UserId}", usuarioId);
                return StatusCode(500, new { message = "Error al obtener historial" });
            }
        }

        // GET api/Historial/{numeroCompra}
        [HttpGet("{numeroCompra:int}")]
        public async Task<IActionResult> GetOrderItems(int numeroCompra)
        {
            try
            {
                var items = new List<OrderItemDto>();
                var connString = _configuration.GetConnectionString("DefaultConnection");

                using (var conn = new SqlConnection(connString))
                using (var cmd = new SqlCommand("usp_GetOrderItems", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroCompra", numeroCompra);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            items.Add(new OrderItemDto
                            {
                                numeroCompra = reader.GetInt32(reader.GetOrdinal("numeroCompra")),
                                productoId = reader.GetInt32(reader.GetOrdinal("productoId")),
                                nombreProducto = reader.IsDBNull(reader.GetOrdinal("nombreProducto")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombreProducto")),
                                cantidad = reader.GetInt32(reader.GetOrdinal("cantidad")),
                                precioUnitario = reader.IsDBNull(reader.GetOrdinal("precioUnitario")) ? 0m : reader.GetDecimal(reader.GetOrdinal("precioUnitario")),
                                totalItem = reader.IsDBNull(reader.GetOrdinal("totalItem")) ? 0m : reader.GetDecimal(reader.GetOrdinal("totalItem")),
                                fechaCompra = reader.IsDBNull(reader.GetOrdinal("fechaCompra")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("fechaCompra"))
                            });
                        }
                    }
                }

                return Ok(items);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al obtener items pedido {OrderId}", numeroCompra);
                return StatusCode(500, new { message = "Error al obtener items del pedido" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener items pedido {OrderId}", numeroCompra);
                return StatusCode(500, new { message = "Error al obtener items del pedido" });
            }
        }
    }
}