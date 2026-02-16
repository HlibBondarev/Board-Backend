using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IUserRepository : IEntityRepositoryBase<string, User>
{
}
