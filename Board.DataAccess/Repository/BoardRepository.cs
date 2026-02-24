using Board.DataAccess.Repository.Api;
using Board.DataAccess.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace Board.DataAccess.Repository;

public class BoardRepository(IConfiguration configuration) : EntityRepositoryBase<long, Models.Board>(configuration), IBoardRepository
{
    public async Task<Models.Board> Create(Models.Board board) =>
        await CreateOrUpdate(board, SqlStatements.ForBoards.Create);

    public async Task<Models.Board> GetById(long id) =>
        await GetById(id, SqlStatements.ForBoards.GetById);

    public async Task<IEnumerable<Models.Board>> GetAll() =>
        await GetAll(SqlStatements.ForBoards.GetAll);

    public async Task<bool> Any(long id) =>
        await Any(id, SqlStatements.ForBoards.Any);

    public async Task<Models.Board> Update(Models.Board board) =>
        await CreateOrUpdate(board, SqlStatements.ForBoards.Update);

    public async Task<bool> Delete(long id) =>
       await Delete(id, SqlStatements.ForBoards.Delete);

    public async Task<string?> GetBoardHierarchyRawAsync(long boardId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId }
        };

        var jsonResult = await ExecuteReaderAsync(
            SqlStatements.ForIssues.GetByBoardId, parameters);

        // Return null if the result is empty, otherwise return the full JSON string
        return string.IsNullOrWhiteSpace(jsonResult) ? null : jsonResult;
    }

    public async Task<IEnumerable<Models.Board>> GetByUserId(string userId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "UserId", userId }
        };

        return await QueryAsync(SqlStatements.ForBoards.GetBoardsByUserId, parameters);
    }
}