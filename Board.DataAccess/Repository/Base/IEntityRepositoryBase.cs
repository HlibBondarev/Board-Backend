using Board.DataAccess.Models.Base;
using Dapper;

namespace Board.DataAccess.Repository.Base;

public interface IEntityRepositoryBase<TKey, TEntity>
   where TEntity : class, IKeyedEntity<TKey>, new()
   where TKey : IEquatable<TKey>
{
    Task<TEntity> CreateOrUpdate(TEntity entity, string sql, Dictionary<string, object>? additionalParams = null);
    Task<TEntity?> GetById(TKey id, string procName);
    Task<IEnumerable<TEntity>> GetAll(string procName);
    Task<IEnumerable<TEntity>> GetByPropValues(string procName, Dictionary<string, object> parameters);
    Task<string?> GetDataInJson(string procName, Dictionary<string, object> parameters);
    Task<bool> Exists(TKey id, string procName);
    Task<bool> Exists(string procName, Dictionary<string, object> parameters);
    Task<bool> Delete(TKey id, string procedureName);
    Task ExecuteCommandInTransaction(string procName, Dictionary<string, object> parameters);
    Task<long> ExecuteQueryInTransaction(string procName, DynamicParameters parameters);
}
