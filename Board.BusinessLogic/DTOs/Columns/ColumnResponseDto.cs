using Board.Common.Extensions;
using Board.DataAccess.Models;

namespace Board.BusinessLogic.DTOs.Columns;

public record ColumnResponseDto(
    long Id,
    string Name,
    string Description,
    int Position,
    string UserId);

public static class ColumnResponseDtoExtensions
{
    public static ColumnResponseDto ToDto(this Column model) => new(
        model.Id,
        model.Name,
        model.Description,
        model.Position,
        model.UserId);

    public static List<ColumnResponseDto> ToDto(this IEnumerable<Column> list)
        => list.MapToList(ToDto);
}