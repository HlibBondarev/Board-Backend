using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IIssueRepository : IEntityRepositoryBase<long, Issue>
{
    Task<bool> MoveIssueAsync(long issueId, long targetColumnId, int targetPosition);
    Task<bool> ReorderIssuesInColumnAsync(long issueId, long targetColumnId);
}
