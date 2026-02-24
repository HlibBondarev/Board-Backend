using Board.Common.Extensions;

namespace Board.BusinessLogic.DTOs.Boards;

public record BoardResponseDto(
    long Id,
    string Title,
    string? Description,
    DateTime CreatedAt,
    string Role);

public static class BoardResponseDtoExtensions
{
    public static BoardResponseDto ToDto(this DataAccess.Models.Board model) => new(
        model.Id,
        model.Title,
        model.Description,
        model.CreatedAt,
        "Admin");

    public static List<BoardResponseDto> ToDto(this IEnumerable<DataAccess.Models.Board> list)
        => list.MapToList(ToDto);
}