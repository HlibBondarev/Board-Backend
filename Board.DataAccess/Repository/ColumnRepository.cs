using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using Board.DataAccess.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace Board.DataAccess.Repository;

public class ColumnRepository(IConfiguration configuration) : EntityRepositoryBase<long, Column>(configuration), IColumnRepository
{
    public async Task<Column> Create(Column column) =>
        await CreateOrUpdate(column, SqlStatements.ForColumns.Create);

    public async Task<Column> GetById(long id) =>
        await GetById(id, SqlStatements.ForColumns.GetById);

    public async Task<IEnumerable<Column>> GetAll() =>
        await GetAll(SqlStatements.ForColumns.GetAll);

    public async Task<bool> Any(long id) =>
        await Any(id, SqlStatements.ForColumns.Any);

    public async Task<Column> Update(Column column) =>
        await CreateOrUpdate(column, SqlStatements.ForColumns.Update);

    public async Task<bool> Delete(long id) =>
       await Delete(id, SqlStatements.ForColumns.Delete);

    public async Task<(IEnumerable<Column> columns, IEnumerable<Issue> issues, IEnumerable<User> users)> GetAllWithIssuesAndUsers()
    {
        IEnumerable<Column> columns = [];
        IEnumerable<Issue> issues = [];
        IEnumerable<User> users = [];

        // 1. Fetch all raw data sets in ONE round-trip to the database
        await QueryMultipleAsync(SqlStatements.ForColumns.GetIssuesByColumnsForUsers, null, async multi =>
        {
            //2. ReadAsync ensures the web server threads are not blocked during I/O
            columns = await multi.ReadAsync<Column>();
            issues = await multi.ReadAsync<Issue>();
            users = await multi.ReadAsync<User>();
        });

        return (columns, issues, users);
    }
}