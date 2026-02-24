using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;


namespace Board.DataAccess.Models;

public class Board : IKeyedEntity<long>
{
    public long Id { get; init; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}
