using Board.BusinessLogic.DTOs.Users;
using Board.Common.Extensions;
using Board.DataAccess.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForUser.Commands;

public record UpdateUserCommand(
    [Required]
    string Id,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [StringLength(20, MinimumLength = 3)]
    string DisplayName,


    DateTime CreatedAt
    ) : IRequest<UserResponseDto>;

public static class UpdateUserCommandExtensions
{
    public static User ToModel(this UpdateUserCommand dto) => new()
    {
        Id = dto.Id,
        Email = dto.Email,
        DisplayName = dto.DisplayName,
        CreatedAt = dto.CreatedAt
    };
    public static List<User> ToModel(this IEnumerable<UpdateUserCommand> list)
        => list.MapToList(ToModel);
}