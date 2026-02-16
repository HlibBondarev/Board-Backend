using Board.Common.Exceptions;
using Board.DataAccess.Models.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Dapper.SqlMapper;

namespace Board.DataAccess.Repository.Base;

public abstract class EntityRepositoryBase<TKey, TEntity>(IConfiguration configuration)
    where TEntity : class, IKeyedEntity<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly string _connectionString = configuration["ConnectionStrings:DefaultConnection"]
        ?? throw new InvalidOperationException("DefaultConnection connection string is missing.");

    protected internal async Task<TEntity> CreateOrUpdate(TEntity entity, string sql)
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

    protected internal async Task<TEntity> GetById(TKey id, string sql)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var entity = await connection.QueryFirstAsync<TEntity>(
            sql,
            new { Id = id }
        );
        _ = entity ?? throw new NotFoundException($"{typeof(TEntity).Name} with Id = {id} not found");

        return entity;
    }

    protected internal async Task<IEnumerable<TEntity>> GetAll(string sql)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var entities = await connection.QueryAsync<TEntity>(
            sql: sql,
            param: null
        );
        _ = entities ?? throw new NotFoundException($"{typeof(TEntity).Name}s not found");

        return entities;
    }

    protected internal async Task<bool> Any(TKey id, string sql)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var isExists = await connection.QueryFirstAsync<bool>(
            sql: sql,
            param: new { Id = id }
        );

        return isExists;
    }

    protected internal async Task<bool> Delete(TKey id, string sql)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        await connection.ExecuteAsync(
            sql: sql,
            param: new { Id = id }
        );

        return true;
    }

    protected internal async Task QueryMultipleAsync(
        string sql,
        object? parameters,
        Func<GridReader, Task> readFunc)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // QueryMultiple is the most efficient way to handle multiple result sets in Dapper
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        // Execute the provided async reading logic
        await readFunc(multi);
    }

    protected internal async Task ExecuteCommandAsync(string sql, Dictionary<string, object> parameters)
    {
        var dbArgs = new DynamicParameters();
        foreach (var pair in parameters)
        {
            dbArgs.Add(pair.Key, pair.Value);
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            try
            {
                await connection.ExecuteAsync(
                    sql: sql,
                    param: dbArgs,
                    transaction: transaction
                );
                transaction.Commit();

                return;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
