using Board.BusinessLogic.DTOs.Users;
using Board.Common.Extensions;
using Board.DataAccess.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Board.BusinessLogic.Features.ForUser.Commands;

public record CreateUserCommand(
    [Required]
    [MaxLength(64)]
    [property: JsonPropertyName("sub")]
    string Id,

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
        Id = dto.Id,
        Email = dto.Email,
        DisplayName = dto.DisplayName ?? dto.Email,
        CreatedAt = DateTime.UtcNow
    };
    public static List<User> ToModel(this IEnumerable<CreateUserCommand> list)
        => list.MapToList(ToModel);
}