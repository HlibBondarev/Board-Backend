using Board.BusinessLogic.DTOs.Columns;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForColumn.Commands;

public record CreateColumnCommand(
    [Required]
    [StringLength(20, MinimumLength = 3)]
    string Name,

    int Position
) : IRequest<ColumnResponseDto>;