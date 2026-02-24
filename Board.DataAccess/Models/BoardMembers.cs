using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

public class BoardMembers
{
    public long BoardId { get; init; }

    [Required]
    [MaxLength(64)]
    public string UserId { get; init; } = null!;

    [AllowedValues("Admin", "User", ErrorMessage = "Invalid role. Allowed values are: Admin, User")]
    public string Role { get; set; } = "User";
}