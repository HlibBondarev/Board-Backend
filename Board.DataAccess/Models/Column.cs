using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

public class Column : IKeyedEntity<int>
{
    public int Id { get; init; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(200, MinimumLength = 10)]
    public string Description { get; set; } = null!;

    public int Position { get; set; }

    public int UserId { get; set; }
}