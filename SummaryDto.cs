namespace AC.Core.Modules.Summaries.DTOs;

// Respuesta pública: nunca exponemos la entidad directamente.
public record SummaryDto(
    Guid Id, string Content, string Source, int NewsCount, DateTime CreatedAt);
