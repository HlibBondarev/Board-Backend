using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IBoardRepository : IEntityRepositoryBase<long, Models.Board>
{
    Task<string?> GetBoardHierarchyRawAsync(long boardId);
    Task<string?> GetByUserId(string boardId);
    Task<Models.Board> CreateWithAdmin(Models.Board board, string userId);
}