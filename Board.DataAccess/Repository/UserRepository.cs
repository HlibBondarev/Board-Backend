using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using Board.DataAccess.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace Board.DataAccess.Repository;

public class UserRepository(IConfiguration configuration) : EntityRepositoryBase<string, User>(configuration), IUserRepository
{
    public async Task<User> Create(User user) =>
        await CreateOrUpdate(user, SqlStatements.ForUsers.Create);

    public async Task<User> Update(User user) =>
        await CreateOrUpdate(user, SqlStatements.ForUsers.Update);

    public async Task<User?> GetById(string id) =>
        await GetById(id, SqlStatements.ForUsers.GetById);

    public async Task<IEnumerable<User>> GetAll() =>
        await GetAll(SqlStatements.ForUsers.GetAll);

    public async Task<bool> Exists(string id) =>
        await Exists(id, SqlStatements.ForUsers.Exists);

    public async Task<bool> Delete(string id) =>
       await Delete(id, SqlStatements.ForUsers.Delete);

    public async Task<bool> UserEmailIsExists(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        var parameters = new Dictionary<string, object>
        {
            { "Email", email }
        };

        return (await GetByPropValues(SqlStatements.ForUsers.GetByEmail, parameters)).Any();
    }
}