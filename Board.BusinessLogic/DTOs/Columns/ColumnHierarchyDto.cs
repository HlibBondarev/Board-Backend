using Board.BusinessLogic.DTOs.Issues;

namespace Board.BusinessLogic.DTOs.Columns;

public class ColumnHierarchyDto
{
    public long Id { get; init; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Position { get; set; }

    public List<IssueWithNamesDto> Issues { get; set; } = [];
}
