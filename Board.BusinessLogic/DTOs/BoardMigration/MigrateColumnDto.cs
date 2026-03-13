using Board.Common.Extensions;
using Board.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.DTOs.BoardMigration;

public record MigrateColumnDto
(
    [Required]
    [StringLength(50, MinimumLength = 3)]
    string Name,

    [Required]
    [StringLength(200, MinimumLength = 10)]
    string Description,

    //[Required]
    //int Position,

    [Required]
    List<MigrateIssueDto> Issues
);

public static class MigrateColumnDtoExtensions
{
    public static Column ToModel(this MigrateColumnDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        //Position = dto.Position,
        Issues = dto.Issues.ToModel()
    };

    public static List<Column> ToModel(this IEnumerable<MigrateColumnDto> list)
        => list.MapToList(ToModel);
}