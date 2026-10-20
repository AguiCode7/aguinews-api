namespace AC.Core.Entities;

/// <summary>
/// Resumen de noticias generado por la IA y enviado desde n8n.
/// </summary>
public class Summary
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public int NewsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
