using Board.BusinessLogic.DTOs.Columns;
using Board.Common.Extensions;
using Board.DataAccess.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForColumn.Commands;

public record CreateColumnCommand(
    [Required]
    [StringLength(50, MinimumLength = 3)]
    string Name,

    [Required]
    [StringLength(200, MinimumLength = 10)]
    string Description,

    [Required]
    int Position,

    [Required]
    long BoardId
) : IRequest<ColumnResponseDto>;

public static class CreateColumnCommandExtensions
{
    public static Column ToModel(this CreateColumnCommand dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Position = dto.Position,
        BoardId = dto.BoardId
    };

    public static List<Column> ToModel(this IEnumerable<CreateColumnCommand> list)
        => list.MapToList(ToModel);
}