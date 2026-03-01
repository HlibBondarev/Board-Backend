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

    public async Task<string?> GetBoardHierarchyRaw(long boardId, string userId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId },
            { "UserId", userId }
        };

        var jsonResult = await ExecuteReaderAsync(
            SqlStatements.ForIssues.GetByBoardId, parameters);

        // Return null if the result is empty, otherwise return the full JSON string
        return string.IsNullOrWhiteSpace(jsonResult) ? null : jsonResult;
    }

    public async Task<string?> GetByUserId(string userId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "UserId", userId }
        };

        var jsonResult = await ExecuteReaderAsync(
           SqlStatements.ForBoards.GetBoardsByUserIdWithRole, parameters);

        // Return null if the result is empty, otherwise return the full JSON string
        return string.IsNullOrWhiteSpace(jsonResult) ? null : jsonResult;
    }

    public async Task<Models.Board> CreateWithAdmin(Models.Board board, string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));
        var parameters = new Dictionary<string, object>
        {
            { "Title", board.Title },
            { "Description", board.Description ?? string.Empty },
            { "CreatedAt", board.CreatedAt },
            { "UserId", userId }
        };

        return await QueryFirstAsync(SqlStatements.ForBoards.CreateWithAdmin, parameters);
    }

    public async Task<bool> CheckBoardMemberExistence(long boardId, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId } ,
            { "Email", email }
        };

        return await ExecuteQueryAsync(SqlStatements.ForBoards.CheckBoardMemberExistence, parameters);
    }

    public async Task AddBoardMember(long boardId, string email, string role)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
        ArgumentException.ThrowIfNullOrEmpty(role, nameof(role));

        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId } ,
            { "Email", email },
            { "Role", role }
        };

        await ExecuteCommandAsync(SqlStatements.ForBoards.AddBoardMember, parameters);
    }
}