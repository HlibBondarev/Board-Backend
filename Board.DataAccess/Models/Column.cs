using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

// Class for the Column (Status) entity
public class Column
{
    public int Id { get; set; } // Status ID

    [StringLength(20)]
    public required string Name { get; set; } // Column name (e.g., "In Progress", up to 20 characters)

    public int Position { get; set; } // Display order (1, 2, 3...)
}