using Board.BusinessLogic.DTOs.Columns;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForColumn.Commands;

public record UpdateColumnCommand(
    [Required]
    int Id,

    [Required]
    [StringLength(20, MinimumLength = 3)]
    string Name,

    int Position) : IRequest<ColumnResponseDto>;