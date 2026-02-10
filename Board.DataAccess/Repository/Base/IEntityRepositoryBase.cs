using Board.DataAccess.Models.Base;

namespace Board.DataAccess.Repository.Base;

public interface IEntityRepositoryBase<TKey, TEntity>
    where TEntity : class, IKeyedEntity<TKey>, new()
    where TKey : IEquatable<TKey>
{
    Task<TEntity> Create(TEntity entity, string sql);
    Task<TEntity> GetById(TKey id, string sql);
    Task<IEnumerable<TEntity>> GetAll(string sql);
    Task<bool> Any(TKey id, string sql);
    Task<TEntity> Update(TEntity entity, string sql);
    Task<bool> Delete(TKey id, string sql);
}
