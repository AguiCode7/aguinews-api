using AC.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AC.Infrastructure.Data;

public class AguiNewsDbContext(DbContextOptions<AguiNewsDbContext> options)
    : DbContext(options)
{
    public DbSet<Summary> Summaries => Set<Summary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas las IEntityTypeConfiguration de este proyecto.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AguiNewsDbContext).Assembly);
    }
}
