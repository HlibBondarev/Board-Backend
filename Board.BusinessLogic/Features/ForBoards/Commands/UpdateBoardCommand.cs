using Board.BusinessLogic.DTOs.Boards;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForBoards.Commands;

public record UpdateBoardCommand(
    [Required]
    [StringLength(100, MinimumLength = 3)]
    string Title,
    [Required]
    [StringLength(200, MinimumLength = 10)]
    string Description
    ) : IRequest<BoardResponseDto>
{
    public long BoardId { get; init; }
}


public static class UpdateBoardCommandExtensions
{
    public static DataAccess.Models.Board SetToModel(this DataAccess.Models.Board model, UpdateBoardCommand dto)
    {
        model.Title = dto.Title;
        model.Description = dto.Description;

        return model;
    }
}