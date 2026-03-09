using Board.DataAccess.Models.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Dapper.SqlMapper;

namespace Board.DataAccess.Repository.Base;

public abstract class EntityRepositoryBase<TKey, TEntity>(IConfiguration configuration) : IEntityRepositoryBase<TKey, TEntity>
    where TEntity : class, IKeyedEntity<TKey>, new()
    where TKey : IEquatable<TKey>
{
    protected readonly string _connectionString = configuration["ConnectionStrings:DefaultConnection"]
        ?? throw new InvalidOperationException("DefaultConnection connection string is missing.");

    public async Task<TEntity> CreateOrUpdate(TEntity entity, string sql, Dictionary<string, object>? additionalParams = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sql, nameof(sql));

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.AddDynamicParams(entity);
                    parameters.RemoveUnused = true;

                    if (additionalParams != null)
                    {
                        foreach (var pair in additionalParams)
                        {
                            parameters.Add(pair.Key, pair.Value);
                        }
                    }

                    var newEntity = await connection.QueryFirstAsync<TEntity>(
                        sql: sql,
                        param: parameters,
                        transaction: transaction,
                        commandType: CommandType.Text
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

    public async Task<TEntity?> GetById(TKey id, string procName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(procName, nameof(procName));

        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(
            sql: procName,
            param: new { Id = id },
            commandType: CommandType.StoredProcedure
        );

        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAll(string procName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(procName, nameof(procName));

        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var entities = await connection.QueryAsync<TEntity>(
            sql: procName,
            param: null,
            commandType: CommandType.StoredProcedure
        );

        return entities ?? [];
    }

    public async Task<IEnumerable<TEntity>> GetByPropValues(string procName, Dictionary<string, object> parameters)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var entities = await connection.QueryAsync<TEntity>(
            sql: procName,
            param: parameters,
            commandType: CommandType.StoredProcedure
        );

        return entities;
    }

    public async Task<string?> GetDataInJson(
    string procName,
    Dictionary<string, object> parameters)
    {
        // Use a local connection to ensure it stays open during the entire operation
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // SQL Server returns FOR JSON results as a sequence of string fragments.
        // QueryAsync<string> will fetch all these fragments into a list.
        var fragments = await connection.QueryAsync<string>(
            procName,
            parameters,
            commandType: CommandType.StoredProcedure
        );

        // Concatenate all fragments into a single JSON string
        var finalJson = string.Concat(fragments);

        return string.IsNullOrWhiteSpace(finalJson) ? null : finalJson;
    }

    public async Task<bool> Exists(TKey id, string procName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(procName, nameof(procName));

        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        var exists = await connection.QueryFirstAsync<bool>(
            sql: procName,
            param: new { Id = id },
            commandType: CommandType.StoredProcedure
        );

        return exists;
    }

    public async Task<bool> Exists(
        string procName,
        Dictionary<string, object> parameters)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();

        var result = await connection.QueryFirstAsync<bool>(
        procName,
        parameters,
        commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<bool> Delete(TKey id, string procName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(procName, nameof(procName));

        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();
        await connection.ExecuteAsync(
            sql: procName,
            param: new { Id = id },
            commandType: CommandType.StoredProcedure
        );

        return true;
    }

    public async Task ExecuteCommandInTransaction(
        string procName,
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
                    sql: procName,
                    param: dbArgs,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
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
