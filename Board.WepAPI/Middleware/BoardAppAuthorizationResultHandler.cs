using Board.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Board.WepAPI.Middleware;

public class BoardAppAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult result)
    {
        if (result.Forbidden && result.AuthorizationFailure != null)
        {
            var reason = result.AuthorizationFailure.FailureReasons.FirstOrDefault();

            if (reason != null)
            {
                var statusCode = GetStatusCodeForReason(reason.Message);
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsJsonAsync(new
                {
                    Status = statusCode,
                    Title = statusCode.GetTitleForStatus(),
                    Detail = reason.Message,
                    Instance = $"{context.Request.Method} {context.Request.Path}"
                });
                return;
            }
        }

        await _defaultHandler.HandleAsync(next, context, policy, result);
    }

    private static int GetStatusCodeForReason(string reason) => reason switch
    {
        string r when r.Contains("Unauthorized user", StringComparison.OrdinalIgnoreCase) => StatusCodes.Status401Unauthorized,
        string r when r.Contains("Bad request", StringComparison.OrdinalIgnoreCase) => StatusCodes.Status400BadRequest,
        string r when r.Contains("Access denied", StringComparison.OrdinalIgnoreCase) => StatusCodes.Status403Forbidden,
        string r when r.Contains("Forbidden", StringComparison.OrdinalIgnoreCase) => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status403Forbidden // Default for auth failures
    };
}