using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace Board.WepAPI.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            // Extract error messages from ModelState
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            // Create a custom ValidationException to be caught by GlobalExceptionHandler
            var exception = new ValidationException("Model validation failed");

            // Attach the errors dictionary to the Data property
            foreach (var error in errors)
            {
                exception.Data.Add(error.Key, error.Value);
            }

            // This will bubble up to the GlobalExceptionHandler middleware
            throw exception;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Not needed for this logic
    }
}