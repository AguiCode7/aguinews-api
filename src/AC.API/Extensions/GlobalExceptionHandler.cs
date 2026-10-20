using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AC.API.Extensions;

// Convierte las excepciones en respuestas ProblemDetails.
// Validación -> 400 con el detalle; otro error -> 500 sin datos internos.
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken ct)
    {
        var problem = exception is ValidationException validationException
            ? new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid data",
                Detail = string.Join(" ",
                    validationException.Errors.Select(e => e.ErrorMessage))
            }
            : new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Unexpected error"
            };

        if (problem.Status == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception");

        context.Response.StatusCode = problem.Status!.Value;
        await context.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }
}
