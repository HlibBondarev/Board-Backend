using Board.DataAccess.Repository.Api;
using Board.DataAccess.Repository.Base;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

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

    public async Task<long> MigrateBoard(Models.Board board, string userId)
    {
        // Prepare DataTable for Columns
        var colTable = new DataTable();
        colTable.Columns.Add("TempId", typeof(long));
        colTable.Columns.Add("Name", typeof(string));
        colTable.Columns.Add("Description", typeof(string));
        colTable.Columns.Add("Position", typeof(int));

        // Prepare DataTable for Issues
        var issueTable = new DataTable();
        issueTable.Columns.Add("TargetColumnTempId", typeof(long));
        issueTable.Columns.Add("Title", typeof(string));
        issueTable.Columns.Add("Description", typeof(string));
        issueTable.Columns.Add("CreateAt", typeof(DateTime));
        issueTable.Columns.Add("DueDate", typeof(DateTime));
        issueTable.Columns.Add("PositionInColumn", typeof(long));
        issueTable.Columns.Add("CreatorId", typeof(string));
        issueTable.Columns.Add("AssigneeId", typeof(string));

        // Add rows with values 
        foreach (var col in board.Columns)
        {
            colTable.Rows.Add(
                col.Id,
                col.Name,
                col.Description,
                col.Position);

            foreach (var issue in col.Issues)
            {
                issueTable.Rows.Add(
                    issue.ColumnId,
                    issue.Title,
                    issue.Description,
                    issue.CreatedAt,
                    issue.DueDate,
                    issue.PositionInColumn,
                    issue.CreatorId,
                    issue.AssigneeId);
            }
        }

        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId);
        parameters.Add("@Title", board.Title);
        parameters.Add("@Description", board.Description);
        parameters.Add("@CreatedAt", board.CreatedAt);
        parameters.Add("@Columns", colTable.AsTableValuedParameter("MigrateColumnType"));
        parameters.Add("@Issues", issueTable.AsTableValuedParameter("MigrateIssueType"));

        return await ExecuteQueryInTransaction(SqlStatements.ForBoards.MigrateDemoBoard, parameters);
    }
}