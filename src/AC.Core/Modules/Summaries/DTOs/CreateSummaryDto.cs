namespace AC.Core.Modules.Summaries.DTOs;

// Datos que envía n8n para registrar un resumen.
public record CreateSummaryDto(string Content, string Source, int NewsCount);
