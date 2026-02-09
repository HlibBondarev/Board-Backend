namespace Board.WepAPI.Exceptions;

// Base exception class to handle custom application errors
public abstract class BaseException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

// 400 Bad Request
public class BadRequestException(string message) : BaseException(message, StatusCodes.Status400BadRequest);

// 401 Unauthorized
public class UnauthorizedException(string message) : BaseException(message, StatusCodes.Status401Unauthorized);

// 403 Forbidden
public class ForbiddenException(string message) : BaseException(message, StatusCodes.Status403Forbidden);

// 404 Not Found
public class NotFoundException(string message) : BaseException(message, StatusCodes.Status404NotFound);
