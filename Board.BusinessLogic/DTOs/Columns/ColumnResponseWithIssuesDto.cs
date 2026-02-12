using Board.BusinessLogic.DTOs.Issues;

namespace Board.BusinessLogic.DTOs.Columns;

public class ColumnResponseWithIssuesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Position { get; set; }
    public int BoardId { get; set; }
    public List<IssueResponseDto> Issues { get; set; } = [];
}
