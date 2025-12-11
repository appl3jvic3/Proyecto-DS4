using Proyecto_API_DS4.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_API_DS4  .Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CarritoCompra> CarritoCompra { get; set; }

        public DbSet<Producto> Producto { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        
    }
}
