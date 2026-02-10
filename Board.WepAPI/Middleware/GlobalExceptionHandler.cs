using Board.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Board.WepAPI.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Log the exception with Serilog
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        // Determine the status code based on exception type
        var (statusCode, message) = exception switch
        {
            // Custom application exceptions (400, 403, 404)
            BaseException customEx => (customEx.StatusCode, customEx.Message),

            // Argument exceptions (400)
            ArgumentNullException => (StatusCodes.Status400BadRequest,
                "Request data is empty. Please check your input data and try again."),

            ArgumentException ex => (StatusCodes.Status400BadRequest,
                string.IsNullOrWhiteSpace(ex.Message) ? "Validation error. Please check your input data and try again." : ex.Message),

            KeyNotFoundException ex => (StatusCodes.Status404NotFound,
                string.IsNullOrWhiteSpace(ex.Message) ? "No entity with this Id was found." : ex.Message),

            // Security & Permissions (401->403)
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,
                "Sorry, you have no rights to do this. Check your input data and try again."),

            // Validation (400)
            ValidationException ex => (StatusCodes.Status400BadRequest, ex.Message),

            SqlException => (StatusCodes.Status500InternalServerError,
                "Database error occurred. Please check your input data or try again later."),

            OptionsValidationException => (StatusCodes.Status500InternalServerError,
                "Server error, options validation error. Please contact support."),

            // Everything else (500)
            _ => (StatusCodes.Status500InternalServerError,
                "Internal Server Error. Please try again later or contact support.")
        };

        // Standardized error response (RFC 7807)
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitleForStatus(statusCode),
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // Return true to signal that the exception has been handled
        return true;
    }

    private static string GetTitleForStatus(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Bad Request",
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status403Forbidden => "Forbidden",
        StatusCodes.Status404NotFound => "Not Found",
        _ => "Internal Server Error"
    };
}