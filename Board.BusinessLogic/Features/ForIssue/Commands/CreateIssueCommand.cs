using Board.BusinessLogic.DTOs.Issues;
using Board.Common.Extensions;
using Board.DataAccess.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForIssue.Commands;

public record CreateIssueCommand(
    [Required]
    [StringLength(200, MinimumLength = 3)]
    string Title,

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    string Description,

    [DataType(DataType.Date)]
    DateTime? DueDate,

    [Required]
    int PositionInColumn,

    [Required]
    long ColumnId,

    [Required]
    string CreatorId,

    string? AssigneeId
) : IRequest<IssueResponseDto>;

public static class CreateIssueCommandExtensions
{
    public static Issue ToModel(this CreateIssueCommand dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description,
        DueDate = dto.DueDate,
        CreatedAt = DateTime.UtcNow,
        PositionInColumn = dto.PositionInColumn,
        ColumnId = dto.ColumnId,
        CreatorId = dto.CreatorId,
        AssigneeId = dto.AssigneeId,
    };

    public static List<Issue> ToModel(this IEnumerable<CreateIssueCommand> list)
        => list.MapToList(ToModel);
}