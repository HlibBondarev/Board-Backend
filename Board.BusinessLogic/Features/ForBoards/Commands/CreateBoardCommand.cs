using Board.BusinessLogic.DTOs.Boards;
using Board.Common.Extensions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForBoards.Commands;

public record CreateBoardCommand(
    [Required]
    [StringLength(100, MinimumLength = 3)]
    string Title,

    [Required]
    [MaxLength(500)]
    string Description,

    [Required]
    [MaxLength(64)]
    //[property: JsonPropertyName("sub")]
    string UserId
) : IRequest<BoardCreateResponseDto>;

public static class CreateBoardCommandExtensions
{
    public static DataAccess.Models.Board ToModel(this CreateBoardCommand dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description,
        CreatedAt = DateTime.UtcNow,
    };

    public static List<DataAccess.Models.Board> ToModel(this IEnumerable<CreateBoardCommand> list)
        => list.MapToList(ToModel);
}