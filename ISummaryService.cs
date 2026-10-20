using AC.Core.Modules.Summaries.DTOs;

namespace AC.Core.Modules.Summaries.Ports;

// Puerto de entrada: lo que el módulo ofrece a la capa API.
public interface ISummaryService
{
    Task<SummaryDto> CreateAsync(CreateSummaryDto dto, CancellationToken ct);
    Task<SummaryDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<SummaryDto>> GetLatestAsync(
        int count, CancellationToken ct);
}
