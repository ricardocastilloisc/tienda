
using Core.Entitis;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TiendaContext : DbContext
{
    public TiendaContext(DbContextOptions<TiendaContext> options) : base(options)
    {

    }

    public DbSet<Producto> Productos { get; set; }
}
