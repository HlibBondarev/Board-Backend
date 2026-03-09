using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IBoardRepository : IEntityRepository<long, Models.Board>
{
    Task<string?> GetBoardHierarchyInJson(long boardId, string userId);
    Task<string?> GetByUserIdInJson(string userId);
    Task<Models.Board> CreateWithAdmin(Models.Board board, string userId);
    Task<bool> CheckBoardMembershipByUserId(long boardId, string userId);
    Task<bool> CheckBoardMembershipByEmail(long boardId, string email);
    Task<bool> CheckBoardMembershipWithRoleByEmail(long boardId, string email, string role);
    Task AddBoardMember(long boardId, string email, string role);
    Task RemoveBoardMember(long boardId, string email);
    Task<bool> CheckUserIsBoardAdmin(long boardId, string userId);
}