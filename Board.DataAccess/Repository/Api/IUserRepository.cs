using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IUserRepository : IEntityRepository<string, User>
{
    Task<bool> UserEmailIsExists(string email);
}
