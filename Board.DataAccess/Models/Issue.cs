using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

public class Issue : IKeyedEntity<long>
{
    [Required]
    public long Id { get; init; }

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = null!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DueDate { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public long PositionInColumn { get; set; }

    [Required]
    public long ColumnId { get; set; }

    [Required]
    public string CreatorId { get; set; } = null!;

    public string? AssigneeId { get; set; }
}