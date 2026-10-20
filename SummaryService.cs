using AC.Core.Entities;
using AC.Core.Modules.Summaries.DTOs;
using AC.Core.Modules.Summaries.Ports;
using AC.Core.Shared.Models;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AC.Core.Modules.Summaries.UseCases;

// Caso de uso: valida, crea y consulta resúmenes.
public class SummaryService(
    ISummaryRepository repository,
    IValidator<CreateSummaryDto> validator,
    IOptions<AppConfig> config) : ISummaryService
{
    public async Task<SummaryDto> CreateAsync(
        CreateSummaryDto dto, CancellationToken ct)
    {
        // Si los datos no son válidos, lanza ValidationException (API: 400).
        await validator.ValidateAndThrowAsync(dto, ct);

        var summary = new Summary
        {
            Id = Guid.NewGuid(),
            Content = dto.Content.Trim(),
            Source = dto.Source.Trim(),
            NewsCount = dto.NewsCount,
            CreatedAt = DateTime.UtcNow // siempre en UTC
        };

        await repository.AddAsync(summary, ct);
        return ToDto(summary);
    }

    public async Task<SummaryDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var summary = await repository.GetByIdAsync(id, ct);
        return summary is null ? null : ToDto(summary);
    }

    public async Task<IReadOnlyList<SummaryDto>> GetLatestAsync(
        int count, CancellationToken ct)
    {
        // El límite máximo viene de la configuración, no del código.
        var limit = Math.Clamp(count, 1, config.Value.MaxSummariesPerQuery);
        var summaries = await repository.GetLatestAsync(limit, ct);
        return summaries.Select(ToDto).ToList();
    }

    private static SummaryDto ToDto(Summary s) =>
        new(s.Id, s.Content, s.Source, s.NewsCount, s.CreatedAt);
}
