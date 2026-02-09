using Board.Common.Extensions;
using Board.DataAccess.Models;

namespace Board.BusinessLogic.DTOs.Users;

public record UserResponseDto(int Id, string Email, string DisplayName);


public static class UserResponseDtoExtensions
{
    public static UserResponseDto ToDto(this User model) => new(
        model.Id,
        model.Email,
        model.DisplayName);

    public static List<UserResponseDto> ToDto(this IEnumerable<User> list)
        => list.MapToList(ToDto);
}