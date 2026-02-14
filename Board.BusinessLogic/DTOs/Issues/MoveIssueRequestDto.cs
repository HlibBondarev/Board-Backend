namespace Board.BusinessLogic.DTOs.Issues;

public record MoveIssueRequestDto(

    int ColumnId,
    int Position
);