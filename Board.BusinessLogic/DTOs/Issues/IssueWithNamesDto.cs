namespace Board.BusinessLogic.DTOs.Issues;

public class IssueWithNamesDto
{
    public long Id { get; init; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public long PositionInColumn { get; set; }
    public long ColumnId { get; init; }
    public string CreatorId { get; set; } = null!;
    public string? AssigneeId { get; set; }
    // Extra fields from join that are not in the Issue entity
    public string CreatorName { get; set; } = null!;
    public string? AssigneeName { get; set; } = null!;
}
