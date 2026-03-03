using Board.Common.Exceptions;
using Board.DataAccess.Models.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Dapper.SqlMapper;

namespace Board.DataAccess.Repository.Base;

public abstract class EntityRepositoryBase<TKey, TEntity>(IConfiguration configuration)
    where TEntity : class, IKeyedEntity<TKey>, new()
    where TKey : IEquatable<TKey>
{
    protected readonly string _connectionString = configuration["ConnectionStrings:DefaultConnection"]
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

    protected internal async Task<TEntity?> GetById(TKey id, string sql)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(
            sql,
            new { Id = id }
        );

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

    protected internal async Task<TEntity> QueryFirstAsync(
        string sql,
        Dictionary<string, object> parameters)
    {
        var dbArgs = new DynamicParameters();
        foreach (var pair in parameters)
        {
            dbArgs.Add(pair.Key, pair.Value);
        }

        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var entity = await connection.QueryFirstAsync<TEntity>(
            sql: sql,
            param: dbArgs
        );

        return entity;
    }

    protected internal async Task<string?> ExecuteReaderAsync(
    string sql,
    Dictionary<string, object> parameters)
    {
        // Use a local connection to ensure it stays open during the entire operation
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // SQL Server returns FOR JSON results as a sequence of string fragments.
        // QueryAsync<string> will fetch all these fragments into a list.
        var fragments = await connection.QueryAsync<string>(
            sql,
            parameters,
            commandType: CommandType.StoredProcedure
        );

        // Concatenate all fragments into a single JSON string
        var finalJson = string.Concat(fragments);

        return string.IsNullOrWhiteSpace(finalJson) ? null : finalJson;
    }

    protected internal async Task<bool> ExecuteQueryAsync(
    string sql,
    Dictionary<string, object> parameters)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();
        try
        {
            var result = await connection.QueryFirstAsync<bool>(
            sql,
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: transaction
        );
            transaction.Commit();

            return result;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    protected internal async Task ExecuteCommandAsync(
        string sql,
        Dictionary<string, object> parameters)
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
