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
    string Description
    ) : IRequest<ColumnResponseDto>
{
    // The BoardID is not part of the primary constructor to keep the JSON body clean
    public int BoardId { get; init; }
}


public static class CreateColumnCommandExtensions
{
    public static Column ToModel(this CreateColumnCommand dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        BoardId = dto.BoardId
    };

    public static List<Column> ToModel(this IEnumerable<CreateColumnCommand> list)
        => list.MapToList(ToModel);
}