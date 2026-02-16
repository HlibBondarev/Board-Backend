using Board.Common.Extensions;
using Board.DataAccess.Models;

namespace Board.BusinessLogic.DTOs.Issues;

public record IssueResponseDto(
    long Id,
    string Title,
    string Description,
    DateTime? DueDate,
    DateTime CreatedAt,
    long PositionInColumn,
    long ColumnId,
    string CreatorId,
    string? AssigneeId);

public static class IssueResponseDtoExtensions
{
    public static IssueResponseDto ToDto(this Issue model) => new(
        model.Id,
        model.Title,
        model.Description,
        model.DueDate,
        model.CreatedAt,
        model.PositionInColumn,
        model.ColumnId,
        model.CreatorId,
        model.AssigneeId);

    public static List<IssueResponseDto> ToDto(this IEnumerable<Issue> list)
        => list.MapToList(ToDto);
}