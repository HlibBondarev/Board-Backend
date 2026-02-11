using Board.BusinessLogic.DTOs.Columns;
using Board.Common.Extensions;
using Board.DataAccess.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForColumn.Commands;

public record UpdateColumnCommand(
    [Required]
    int Id,

    [Required]
    [StringLength(50, MinimumLength = 3)]
    string Name,

    [Required]
    [StringLength(200, MinimumLength = 10)]
    string Description,

    [Required]
    int Position,

    [Required]
    string UserId
    ) : IRequest<ColumnResponseDto>;

public static class UpdateColumnCommandExtensions
{
    public static Column ToModel(this UpdateColumnCommand dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Description = dto.Description,
        Position = dto.Position,
        UserId = dto.UserId
    };

    public static List<Column> ToModel(this IEnumerable<UpdateColumnCommand> list)
        => list.MapToList(ToModel);
}