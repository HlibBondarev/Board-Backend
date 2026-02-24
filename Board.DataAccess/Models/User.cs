using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

public class User : IKeyedEntity<string>
{
    [Required]
    [MaxLength(64)]
    public string Id { get; init; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string DisplayName { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }
}