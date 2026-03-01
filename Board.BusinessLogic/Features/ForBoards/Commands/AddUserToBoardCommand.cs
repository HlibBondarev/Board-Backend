using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForBoards.Commands;

public record AddUserToBoardCommand(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Role)
    : IRequest
{
    [Required]
    [MaxLength(64)]
    public long BoardId;
};