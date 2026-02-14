using Board.BusinessLogic.DTOs.Issues;

namespace Board.BusinessLogic.DTOs.Columns;

public record ColumnResponseWithIssuesDto(
    int Id,
    string Name,
    string Description,
    int Position,
    string UserId,
    string UserDisplayName,
    List<IssueWithUserNameResponseDto> Issues
    );
