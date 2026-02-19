using Board.BusinessLogic.DTOs.Issues;

namespace Board.BusinessLogic.DTOs.Columns;

public record ColumnWithIssuesAndUserResponseDto(
    long Id,
    string Name,
    string Description,
    int Position,
    string UserId,
    string UserDisplayName,
    List<IssueWithUserByColumnsResponseDto> Issues
    );
