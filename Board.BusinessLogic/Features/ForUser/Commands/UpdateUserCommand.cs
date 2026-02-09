using Board.BusinessLogic.DTOs.Users;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForUser.Commands;

public record UpdateUserCommand(
    [Required]
    int Id,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [StringLength(20, MinimumLength = 3)]
    string DisplayName) : IRequest<UserResponseDto>;