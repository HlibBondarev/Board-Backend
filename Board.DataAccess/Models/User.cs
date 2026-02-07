using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

// Class for the User entity
public class User : IKeyedEntity<int>
{
    public int Id { get; init; }
    public required string Email { get; set; }
    [StringLength(20, MinimumLength = 3)]
    public required string DisplayName { get; set; }
}