using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

public class Issue : IKeyedEntity<int>
{
    public int Id { get; init; }

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = null!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int PositionInColumn { get; set; }

    public int ColumnId { get; set; }

    public int CreatorId { get; set; }

    public int? AssigneeId { get; set; }
}
