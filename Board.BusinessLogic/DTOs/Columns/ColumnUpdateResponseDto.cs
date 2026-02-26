using Board.Common.Extensions;
using Board.DataAccess.Models;

namespace Board.BusinessLogic.DTOs.Columns;

public record ColumnUpdateResponseDto(
    long Id,
    string Name,
    string Description);

public static class ColumnUpdateResponseDtoExtensions
{
    public static ColumnUpdateResponseDto ToUpdateDto(this Column model) => new(
        model.Id,
        model.Name,
        model.Description);

    public static List<ColumnUpdateResponseDto> ToUpdateDto(this IEnumerable<Column> list)
        => list.MapToList(ToUpdateDto);
}