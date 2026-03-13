using Board.Common.Extensions;
using Board.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.DTOs.BoardMigration;

public record MigrateIssueDto
(
    [Required]
    [StringLength(200, MinimumLength = 3)]
    string Title,

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    string Description,

    [DataType(DataType.Date)]
    DateTime? DueDate

//[Required]
//int PositionInColumn
);

public static class MigrateIssueDtoExtensions
{
    public static Issue ToModel(this MigrateIssueDto dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description,
        DueDate = dto.DueDate,
        CreatedAt = DateTime.UtcNow,
        //PositionInColumn = dto.PositionInColumn,
        //ColumnId = dto.ColumnId,
        //CreatorId = dto.CreatorId,
        //AssigneeId = dto.AssigneeId,
    };

    public static List<Issue> ToModel(this IEnumerable<MigrateIssueDto> list)
        => list.MapToList(ToModel);
}