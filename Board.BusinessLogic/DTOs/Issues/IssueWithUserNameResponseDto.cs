namespace Board.BusinessLogic.DTOs.Issues;

public record IssueWithUserNameResponseDto(
    int Id,
    string Title,
    string Description,
    DateTime? DueDate,
    DateTime CreatedAt,
    int PositionInColumn,
    int ColumnId,
    string CreatorId,
    string CreatorName,
    string? AssigneeId,
    string? AssigneeName);