using Board.Common.Exceptions;
using Board.DataAccess.Models.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Dapper.SqlMapper;

namespace Board.DataAccess.Repository.Base;

public class EntityRepositoryBase<TKey, TEntity>(IConfiguration configuration) : IEntityRepositoryBase<TKey, TEntity>
    where TEntity : class, IKeyedEntity<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly string _connectionString = configuration["ConnectionStrings:DefaultConnection"]
        ?? throw new InvalidOperationException("DefaultConnection connection string is missing.");

    public async Task<TEntity> Create(TEntity entity, string sql)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var newEntity = await connection.QueryFirstAsync<TEntity>(
                        sql,
                        entity,
                        transaction: transaction
                    );
                    transaction.Commit();

                    return newEntity;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<TEntity> GetById(TKey id, string sql)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(
            sql,
            new { Id = id }
        );
        _ = entity ?? throw new NotFoundException($"{typeof(TEntity).Name} with Id = {id} not found");

        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAll(string sql)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var entities = await connection.QueryAsync<TEntity>(
            sql: sql,
            param: null
        );

        return entities;
    }

    public async Task<bool> Any(TKey id, string sql)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var isExists = await connection.QueryFirstAsync<bool>(
            sql: sql,
            param: new { Id = id }
        );

        return isExists;
    }

    public async Task<TEntity> Update(TEntity entity, string sql)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var newEntity = await connection.QueryFirstAsync<TEntity>(
                        sql,
                        entity,
                        transaction: transaction
                    );
                    transaction.Commit();

                    return newEntity;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public async Task<bool> Delete(TKey id, string sql)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        await connection.ExecuteAsync(
            sql: sql,
            param: new { Id = id }
        );

        return true;
    }
}
