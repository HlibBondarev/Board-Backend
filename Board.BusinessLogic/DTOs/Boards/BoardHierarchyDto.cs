using Board.BusinessLogic.DTOs.Columns;

namespace Board.BusinessLogic.DTOs.Boards;

public class BoardHierarchyDto
{
    public long Id { get; init; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ColumnHierarchyDto> Columns { get; set; } = [];
}