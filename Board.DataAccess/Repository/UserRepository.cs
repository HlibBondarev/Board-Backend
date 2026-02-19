using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using Board.DataAccess.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace Board.DataAccess.Repository;

public class UserRepository(IConfiguration configuration) : EntityRepositoryBase<string, User>(configuration), IUserRepository
{
    public async Task<User> Create(User user) =>
        await CreateOrUpdate(user, SqlStatements.ForUsers.Create);

    public async Task<bool> Any(string id) =>
        await Any(id, SqlStatements.ForUsers.Any);

    public async Task<User> GetById(string id) =>
        await GetById(id, SqlStatements.ForUsers.GetById);

    public async Task<User> Update(User user) =>
        await CreateOrUpdate(user, SqlStatements.ForUsers.Update);

    public async Task<bool> Delete(string id) =>
       await Delete(id, SqlStatements.ForUsers.Delete);

    public async Task<IEnumerable<User>> GetAll() =>
        await GetAll(SqlStatements.ForUsers.GetAll);
}