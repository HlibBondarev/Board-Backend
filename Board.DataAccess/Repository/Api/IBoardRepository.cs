using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IBoardRepository : IEntityRepositoryBase<long, Models.Board>
{
    public Task<string?> GetBoardHierarchyRawAsync(long boardId);
    public Task<IEnumerable<Models.Board>> GetByUserId(string boardId);
}