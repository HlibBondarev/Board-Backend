namespace Board.BusinessLogic.DTOs.Issues;

public record IssuesByColumnIdResponseDto(
    long ColumnId,
    IEnumerable<IssueWithUserByColumnsResponseDto> Issues);