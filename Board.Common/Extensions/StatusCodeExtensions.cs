using Microsoft.AspNetCore.Http;

namespace Board.Common.Extensions;

public static class StatusCodeExtensions
{
    public static string GetTitleForStatus(this int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Bad Request",
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status403Forbidden => "Forbidden",
        StatusCodes.Status404NotFound => "Not Found",
        _ => "Internal Server Error"
    };
}