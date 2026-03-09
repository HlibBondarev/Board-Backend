using Board.Common.Extensions;
using Board.DataAccess.Models;

namespace Board.BusinessLogic.DTOs.Columns;

public record ColumnCreateResponseDto(
    long Id,
    int Position);

public static class ColumnCreateResponseDtoExtensions
{
    public static ColumnCreateResponseDto ToCreateDto(this Column model) => new(
        model.Id,
        model.Position);

    public static List<ColumnCreateResponseDto> ToCreateDto(this IEnumerable<Column> list)
        => list.MapToList(ToCreateDto);
}