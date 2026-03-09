using Board.Common.Exceptions;
using Board.Common.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;

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
        var (statusCode, _) = exception switch
        {
            // Custom application exceptions (400, 403, 404)
            BaseException customEx => (customEx.StatusCode, customEx.Message),

            // Argument exceptions (400)
            ArgumentNullException => (StatusCodes.Status400BadRequest,
                "Request data is empty. Please check your input data and try again."),

            InvalidOperationException => (StatusCodes.Status400BadRequest,
                string.IsNullOrWhiteSpace(exception.Message)
                ? "Invalid operation. Please check your input data and try again."
                : exception.Message),

            ArgumentException ex => (StatusCodes.Status400BadRequest,
                string.IsNullOrWhiteSpace(ex.Message)
                ? "Validation error. Please check your input data and try again."
                : ex.Message),

            AuthenticationException ex => (StatusCodes.Status400BadRequest,
                string.IsNullOrWhiteSpace(ex.Message)
                ? $"Can not get user's claim from Context."
                : ex.Message),

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

        // 1. Extract and format validation errors if they exist
        string? detailedMessage = null;

        if (exception is ValidationException && exception.Data.Count > 0)
        {
            var errorList = new List<string>();
            foreach (System.Collections.DictionaryEntry entry in exception.Data)
            {
                if (entry.Value is string[] messages)
                {
                    // Format each field as "Field: error1, error2"
                    errorList.Add($"{entry.Key}: {string.Join(", ", messages)}");
                }
            }

            // Join all field errors into one single string
            if (errorList.Count != 0)
            {
                detailedMessage = string.Join(" | ", errorList);
            }
        }

        // 2. Create ProblemDetails using the detailed message for 'Detail'
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode.GetTitleForStatus(),
            Detail = detailedMessage ?? exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        // 3. Keep the original 'errors' extension (optional, for future flexibility)
        if (exception is ValidationException && exception.Data.Count > 0)
        {
            problemDetails.Extensions["errors"] = exception.Data;
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}