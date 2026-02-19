namespace Board.BusinessLogic.DTOs.Issues;

public record IssueWithUserByColumnsResponseDto(
    long Id,
    string Title,
    string Description,
    DateTime? DueDate,
    DateTime CreatedAt,
    long PositionInColumn,
    long ColumnId,
    string CreatorId,
    string CreatorName,
    string? AssigneeId,
    string? AssigneeName);