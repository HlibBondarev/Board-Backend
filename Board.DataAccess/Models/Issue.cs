using Board.DataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Board.DataAccess.Models;

// Class for the Task entity
public class Issue : IKeyedEntity<int>
{
    public int Id { get; init; } // Task ID

    [StringLength(200, MinimumLength = 3)]
    public required string Title { get; set; } // Task title (from 3 to 200 characters)

    [StringLength(2000, MinimumLength = 10)]
    public required string Description { get; set; } // Detailed text (from 10 to 2000 characters)

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DueDate { get; set; } // Deadline (date for calendar), can be null

    public int ColumnId { get; set; } // Current status (Foreign Key to Columns.Id)

    public DateTime CreatedAt { get; set; } // Date and time of task creation

    public int CreatorId { get; set; } // The person who created the task (Foreign Key to Users.Id)

    public int? AssigneeId { get; set; } // Assigned person (Foreign Key to Users.Id, can be null)
}
