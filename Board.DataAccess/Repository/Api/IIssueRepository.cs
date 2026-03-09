using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IIssueRepository : IEntityRepository<long, Issue>
{
    Task<IEnumerable<Issue>> GetIssuesInColumn(long columnId);
    Task<bool> MoveIssueAsync(long issueId, long targetColumnId, int targetPosition);
    Task<bool> ReorderIssuesInColumnAsync(long issuePosition, long columnId);
}
