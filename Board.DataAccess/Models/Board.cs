using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;


namespace Board.DataAccess.Models;

public class Board : IKeyedEntity<long>
{
    public long Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string? Description { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public IEnumerable<Column> Columns { get; set; } = [];
}
