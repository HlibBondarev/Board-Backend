using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

public class User : IKeyedEntity<int>
{
    public int Id { get; init; }

    public string Email { get; set; } = null!;

    [StringLength(20, MinimumLength = 3)]
    public string DisplayName { get; set; } = null!;
}