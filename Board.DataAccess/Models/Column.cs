using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

public class Column : IKeyedEntity<long>
{
    public long Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(200, MinimumLength = 10)]
    public string Description { get; set; } = null!;

    [Required]
    public int Position { get; set; }

    [Required]
    public long BoardId { get; init; }

    // Navigation property
    public IEnumerable<Issue> Issues { get; set; } = [];
}