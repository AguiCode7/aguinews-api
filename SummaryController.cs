using AC.Core.Modules.Summaries.DTOs;
using AC.Core.Modules.Summaries.Ports;
using Microsoft.AspNetCore.Mvc;

namespace AC.API.Controllers;

// Endpoints de resúmenes: solo recibe y responde, la lógica está en Core.
[ApiController]
[Route("api/summaries")]
public class SummaryController(ISummaryService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<SummaryDto>> Create(
        CreateSummaryDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SummaryDto>> GetById(
        Guid id, CancellationToken ct)
    {
        var summary = await service.GetByIdAsync(id, ct);
        return summary is null ? NotFound() : Ok(summary);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SummaryDto>>> GetLatest(
        [FromQuery] int count = 10, CancellationToken ct = default)
        => Ok(await service.GetLatestAsync(count, ct));
}
