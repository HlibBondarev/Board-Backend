using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IBoardRepository : IEntityRepositoryBase<long, Models.Board>
{
    Task<string?> GetBoardHierarchyRaw(long boardId, string userId);
    Task<string?> GetByUserId(string boardId);
    Task<Models.Board> CreateWithAdmin(Models.Board board, string userId);
    Task<bool> CheckBoardMemberExistence(long boardId, string email);
    Task AddBoardMember(long boardId, string email, string role);
    Task RemoveBoardMember(long boardId, string email);
}