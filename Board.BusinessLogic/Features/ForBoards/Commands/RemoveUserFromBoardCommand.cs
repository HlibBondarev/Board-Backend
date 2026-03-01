using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForBoards.Commands;

public record RemoveUserFromBoardCommand(
    [Required]
    [EmailAddress]
    string Email)
    : IRequest
{
    [Required]
    public long BoardId;
};