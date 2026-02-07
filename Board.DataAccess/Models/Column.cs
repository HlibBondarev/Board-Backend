using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

// Class for the Column (Status) entity
public class Column : IKeyedEntity<int>
{
    public int Id { get; init; } // Status ID

    [StringLength(20)]
    public required string Name { get; set; } = null!;// Column name (e.g., "In Progress", up to 20 characters)

    public required int Position { get; set; } // Display order (1, 2, 3...)
}