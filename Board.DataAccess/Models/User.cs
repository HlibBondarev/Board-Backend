using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

// Class for the User entity
public class User
{
    public int Id { get; set; } // Unique identifier (Primary Key)
    public required string Email { get; set; } // Email from OAuth

    [StringLength(20, MinimumLength = 3)]
    public required string DisplayName { get; set; } // Username (from 3 to 20 characters)
}