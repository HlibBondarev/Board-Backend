using Board.DataAccess.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForColumn.Commands;

public record UpdateColumnCommand(
    [Required]
    [StringLength(50, MinimumLength = 3)]
    string Name,
    [Required]
    [StringLength(200, MinimumLength = 10)]
    string Description
    ) : IRequest<bool>
{
    public long ColumnId { get; init; }
}


public static class UpdateColumnCommandExtensions
{
    public static Column SetToModel(this Column model, UpdateColumnCommand dto)
    {
        model.Name = dto.Name;
        model.Description = dto.Description;

        return model;
    }
}