using Board.DataAccess.Repository.Api;
using Board.DataAccess.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace Board.DataAccess.Repository;

public class BoardRepository(IConfiguration configuration) : EntityRepositoryBase<long, Models.Board>(configuration), IBoardRepository
{
    public async Task<Models.Board> Create(Models.Board board) =>
        await CreateOrUpdate(board, SqlStatements.ForBoards.Create);

    public async Task<Models.Board> CreateWithAdmin(Models.Board board, string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var additionalParams = new Dictionary<string, object>
        {
            { "UserId", userId }
        };

        return await CreateOrUpdate(board, SqlStatements.ForBoards.CreateWithAdmin, additionalParams);
    }

    public async Task<Models.Board> Update(Models.Board board) =>
        await CreateOrUpdate(board, SqlStatements.ForBoards.Update);

    public async Task<Models.Board?> GetById(long id) =>
        await GetById(id, SqlStatements.ForBoards.GetById);

    public async Task<IEnumerable<Models.Board>> GetAll() =>
        await GetAll(SqlStatements.ForBoards.GetAll);

    public async Task<bool> Exists(long id) =>
        await Exists(id, SqlStatements.ForBoards.Exists);

    public async Task<bool> Delete(long id) =>
       await Delete(id, SqlStatements.ForBoards.Delete);

    public async Task<string?> GetBoardHierarchyInJson(long boardId, string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId },
            { "UserId", userId }
        };

        var jsonResult = await GetDataInJson(
            SqlStatements.ForIssues.GetByBoardIdInJson, parameters);

        // Return null if the result is empty, otherwise return the full JSON string
        return string.IsNullOrWhiteSpace(jsonResult) ? null : jsonResult;
    }

    public async Task<string?> GetByUserIdInJson(string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        var parameters = new Dictionary<string, object>
        {
            { "UserId", userId }
        };

        var jsonResult = await GetDataInJson(
           SqlStatements.ForBoards.GetBoardsByUserIdWithRoleInJson, parameters);

        // Return null if the result is empty, otherwise return the full JSON string
        return string.IsNullOrWhiteSpace(jsonResult) ? null : jsonResult;
    }

    public async Task<bool> CheckBoardMembershipByUserId(long boardId, string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));

        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId } ,
            { "UserId", userId }
        };

        return await Exists(SqlStatements.ForBoards.CheckBoardMembershipByUserId, parameters);
    }

    public async Task<bool> CheckBoardMembershipByEmail(long boardId, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId },
            { "Email", email }
        };

        return await Exists(SqlStatements.ForBoards.CheckBoardMembershipByEmail, parameters);
    }

    public async Task<bool> CheckBoardMembershipWithRoleByEmail(long boardId, string email, string role)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
        ArgumentException.ThrowIfNullOrEmpty(role, nameof(role));

        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId },
            { "Email", email },
            { "Role", role }
        };

        return await Exists(SqlStatements.ForBoards.CheckBoardMembershipWithRoleByEmail, parameters);
    }

    public async Task<bool> CheckUserIsBoardAdmin(long boardId, string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));

        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId } ,
            { "UserId", userId }
        };

        return await Exists(SqlStatements.ForBoards.CheckUserIsBoardAdmin, parameters);
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

        await ExecuteCommandInTransaction(SqlStatements.ForBoards.AddBoardMember, parameters);
    }

    public async Task RemoveBoardMember(long boardId, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        var parameters = new Dictionary<string, object>
        {
            { "BoardId", boardId } ,
            { "Email", email }
        };

        await ExecuteCommandInTransaction(SqlStatements.ForBoards.RemoveBoardMember, parameters);
    }
}