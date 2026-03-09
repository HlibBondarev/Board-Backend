using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using Board.DataAccess.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace Board.DataAccess.Repository;

public class IssueRepository(IConfiguration configuration) : EntityRepositoryBase<long, Issue>(configuration), IIssueRepository
{
    public async Task<Issue> Create(Issue issue) =>
        await CreateOrUpdate(issue, SqlStatements.ForIssues.Create);

    public async Task<Issue> Update(Issue issue) =>
        await CreateOrUpdate(issue, SqlStatements.ForIssues.Update);

    public async Task<Issue?> GetById(long id) =>
        await GetById(id, SqlStatements.ForIssues.GetById);

    public async Task<IEnumerable<Issue>> GetAll() =>
        await GetAll(SqlStatements.ForIssues.GetAll);

    public async Task<bool> Exists(long id) =>
        await Exists(id, SqlStatements.ForIssues.Exists);

    public async Task<bool> Delete(long id) =>
       await Delete(id, SqlStatements.ForIssues.Delete);

    public async Task<IEnumerable<Issue>> GetIssuesInColumn(long columnId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "ColumnId", columnId }
        };

        return await GetByPropValues(SqlStatements.ForIssues.GetByColumnId, parameters);
    }

    public async Task<bool> MoveIssueAsync(long issueId, long targetColumnId, int targetPosition)
    {
        var parameters = new Dictionary<string, object>
        {
            { "IssueId", issueId },
            { "TargetColumnId", targetColumnId },
            { "NewPosition", targetPosition}
        };

        await ExecuteCommandInTransaction(SqlStatements.ForIssues.MoveIssue, parameters);

        return true;
    }

    public async Task<bool> ReorderIssuesInColumnAsync(long issuePosition, long columnId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "IssuePosition", issuePosition },
            { "ColumnId", columnId }
        };

        await ExecuteCommandInTransaction(SqlStatements.ForIssues.ReorderIssuesInColumn, parameters);

        return true;
    }
}