namespace Board.BusinessLogic.DTOs.Issues;

public record IssueWithCreatorAndAssigneeResponseDto(
    long Id,
    string Title,
    string Description,
    DateTime? DueDate,
    DateTime CreatedAt,
    long PositionInColumn,
    long ColumnId,
    string CreatorId,
    string? AssigneeId,
    string CreatorName,
    string? AssigneeName);