namespace Board.Common.Exceptions;

public abstract class BaseException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

// 400 Bad Request
public class BadRequestException(string message = "Bad Request")
    : BaseException(message, 400);

// 401 Unauthorized
public class UnauthorizedException(string message = "Unauthorized access")
    : BaseException(message, 401);

// 403 Forbidden
public class ForbiddenException(string message = "Access forbidden")
    : BaseException(message, 403);

// 404 Not Found
public class NotFoundException(string message = "The requested resource was not found")
    : BaseException(message, 404);
