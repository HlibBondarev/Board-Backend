using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;

namespace Board.DataAccess.Repository.Api;

public interface IColumnRepository : IEntityRepositoryBase<long, Column>
{
    Task<(IEnumerable<Column> columns, IEnumerable<Issue> issues, IEnumerable<User> users)> GetAllWithIssuesAndUsers();
}