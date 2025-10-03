using Microsoft.EntityFrameworkCore;
using DesvendandoMinimalAPI.Domain;

namespace DesvendandoMinimalAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
    }
}
