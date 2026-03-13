using Board.BusinessLogic.DTOs.BoardMigration;
using Board.Common.Extensions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForBoards.Commands;

public record MigrateBoardCommand
(
    // Navigation property
    List<MigrateColumnDto> Columns,

    [Required]
    [StringLength(100, MinimumLength = 3)]
    string Title  = "Board imported from Demo",

    [Required]
    [StringLength(500, MinimumLength = 10)]
    string Description = "Description of board imported from Demo"
) : IRequest<long>
{
    [Required]
    [MaxLength(64)]
    public required string UserId;
};

public static class MigrateBoardCommandExtensions
{
    public static DataAccess.Models.Board ToModel(this MigrateBoardCommand dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description,
        CreatedAt = DateTime.UtcNow,
        Columns = dto.Columns.ToModel()
    };

    public static List<DataAccess.Models.Board> ToModel(this IEnumerable<MigrateBoardCommand> list)
        => list.MapToList(ToModel);
}