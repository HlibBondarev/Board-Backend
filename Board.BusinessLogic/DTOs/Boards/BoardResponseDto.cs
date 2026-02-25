namespace Board.BusinessLogic.DTOs.Boards;

public record BoardResponseDto(
    long Id,
    string Title,
    string? Description,
    DateTime CreatedAt,
    string Role);