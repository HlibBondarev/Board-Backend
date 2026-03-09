using Board.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Board.WepAPI.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ValidateIdAttribute(string paramName = "id") : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // 1. Check if the specified parameter exists in RouteValues
        if (!context.RouteData.Values.TryGetValue(paramName, out var value) ||
            !long.TryParse(value?.ToString(), out _))
        {
            // 2. Return 400 Bad Request if validation fails
            context.Result = new BadRequestObjectResult(new
            {
                Status = StatusCodes.Status400BadRequest,
                Title = StatusCodes.Status400BadRequest.GetTitleForStatus(),
                Detail = $"Invalid or missing identifier: '{paramName}'. Expected an integer.",
                Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}"
            });
        }
    }
}
