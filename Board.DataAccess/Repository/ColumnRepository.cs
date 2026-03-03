using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using Board.DataAccess.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace Board.DataAccess.Repository;

public class ColumnRepository(IConfiguration configuration) : EntityRepositoryBase<long, Column>(configuration), IColumnRepository
{
    public async Task<Column> Create(Column column) =>
        await CreateOrUpdate(column, SqlStatements.ForColumns.Create);

    public async Task<Column?> GetById(long id) =>
        await GetById(id, SqlStatements.ForColumns.GetById);

    public async Task<IEnumerable<Column>> GetAll() =>
        await GetAll(SqlStatements.ForColumns.GetAll);

    public async Task<bool> Any(long id) =>
        await Any(id, SqlStatements.ForColumns.Any);

    public async Task<Column> Update(Column column) =>
        await CreateOrUpdate(column, SqlStatements.ForColumns.Update);

    public async Task<bool> Delete(long id) =>
       await Delete(id, SqlStatements.ForColumns.Delete);

    public async Task<string?> GetIssuesInColumnRaw(long columnId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "ColumnId", columnId }
        };

        var jsonResult = await ExecuteReaderAsync(
            SqlStatements.ForIssues.GetByColumnIdWithUsers, parameters);

        // Return null if the result is empty, otherwise return the full JSON string
        return string.IsNullOrWhiteSpace(jsonResult) ? null : jsonResult;
    }

    public async Task<bool> ReorderColumnsInBoard(int columnPosition, long boardId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "ColumnPosition", columnPosition },
            { "BoardId", boardId }
        };

        await ExecuteCommandAsync(SqlStatements.ForColumns.ReorderColumnsInBoard, parameters);

        return true;
    }
}
