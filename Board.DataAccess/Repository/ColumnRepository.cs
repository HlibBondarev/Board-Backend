using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace Board.DataAccess.Repository;

public class ColumnRepository(IConfiguration configuration) : EntityRepositoryBase<int, Column>(configuration)
{
    private readonly string _connectionString = configuration["ConnectionStrings:DefaultConnection"]
        ?? throw new InvalidOperationException("DefaultConnection connection string is missing.");

    //public async Task<IEnumerable<ColumnResponseWithIssuesDto>> GetAllWithIssues()
    //{

    //}
}
