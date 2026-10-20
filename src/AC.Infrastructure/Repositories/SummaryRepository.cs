using AC.Core.Entities;
using AC.Core.Modules.Summaries.Ports;
using AC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AC.Infrastructure.Repositories;

// Adaptador: implementa el puerto ISummaryRepository con EF Core.
public class SummaryRepository(AguiNewsDbContext db) : ISummaryRepository
{
    public async Task AddAsync(Summary summary, CancellationToken ct)
    {
        db.Summaries.Add(summary);
        await db.SaveChangesAsync(ct);
    }

    // Solo lectura: AsNoTracking evita el seguimiento de cambios.
    public Task<Summary?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.Summaries.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IReadOnlyList<Summary>> GetLatestAsync(
        int count, CancellationToken ct) =>
        await db.Summaries.AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .Take(count)
            .ToListAsync(ct);
}
