namespace Board.BusinessLogic.DTOs.Issues;

public record MoveIssueRequestDto(

    long ColumnId,
    int Position
);