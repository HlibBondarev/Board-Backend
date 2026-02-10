using Board.BusinessLogic.DTOs.Users;
using Board.Common.Extensions;
using Board.DataAccess.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForUser.Commands;

public record CreateUserCommand(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [StringLength(20, MinimumLength = 3)]
    string DisplayName
) : IRequest<UserResponseDto>;

public static class CreateUserCommandExtensions
{
    public static User ToModel(this CreateUserCommand dto) => new()
    {
        Email = dto.Email,
        DisplayName = dto.DisplayName,
        CreatedAt = DateTime.UtcNow
    };
    public static List<User> ToModel(this IEnumerable<CreateUserCommand> list)
        => list.MapToList(ToModel);
}