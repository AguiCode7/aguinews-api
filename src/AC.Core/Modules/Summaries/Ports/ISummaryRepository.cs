using AC.Core.Entities;

namespace AC.Core.Modules.Summaries.Ports;

// Puerto de salida: qué necesita el módulo para persistir, sin decir cómo.
// La implementación vive en AC.Infrastructure.
public interface ISummaryRepository
{
    Task AddAsync(Summary summary, CancellationToken ct);
    Task<Summary?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Summary>> GetLatestAsync(int count, CancellationToken ct);
}
