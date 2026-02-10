using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

public class User : IKeyedEntity<int>
{
    [Required]
    public int Id { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string DisplayName { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }
}