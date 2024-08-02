using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.Application.Exceptions;

namespace PlaceRentalApp.API.Middlewares
{
    public class ApiExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails? details;

            if (exception is NotFoundExceptions)
            {
                details = new ProblemDetails
                {
                    Title = exception.Message,
                    Status = StatusCodes.Status404NotFound
                };
            }
            else
            {
                details = new ProblemDetails
                {
                    Title = "Server Error",
                    Status = StatusCodes.Status500InternalServerError
                };
            }

            httpContext.Response.StatusCode = details.Status ?? StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

            return true;
        }
    }
}
