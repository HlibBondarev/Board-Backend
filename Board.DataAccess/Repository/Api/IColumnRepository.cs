using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IColumnRepository : IEntityRepositoryBase<long, Column>
{
    Task<string?> GetIssuesInColumnRaw(long boardId);
    Task<bool> ReorderColumnsInBoard(int columnPosition, long boardId);
}