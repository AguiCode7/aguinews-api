using AC.Core.Modules.Summaries.DTOs;
using FluentValidation;

namespace AC.Core.Modules.Summaries.Validators;

// Reglas de validación del resumen que llega desde n8n.
public class CreateSummaryValidator : AbstractValidator<CreateSummaryDto>
{
    public const int SourceMaxLength = 200;

    public CreateSummaryValidator()
    {
        RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
        RuleFor(x => x.Source).NotEmpty().MaximumLength(SourceMaxLength);
        RuleFor(x => x.NewsCount).GreaterThan(0);
    }
}
