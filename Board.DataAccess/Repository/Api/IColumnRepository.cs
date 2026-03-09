using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IColumnRepository : IEntityRepository<long, Column>
{
    Task<string?> GetIssuesInColumnInJson(long columnId);
    Task<long> GetBoardIdByColumnId(long columnId);
    Task<bool> ReorderColumnsInBoard(int columnPosition, long boardId);
}