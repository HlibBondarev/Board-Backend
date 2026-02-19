using Board.DataAccess.Models.Base;

namespace Board.DataAccess.Repository.Base;

public interface IEntityRepositoryBase<TKey, TEntity>
   where TEntity : class, IKeyedEntity<TKey>, new()
   where TKey : IEquatable<TKey>
{
    Task<TEntity> Create(TEntity entity);
    Task<TEntity> GetById(TKey id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<bool> Any(TKey id);
    Task<TEntity> Update(TEntity entity);
    Task<bool> Delete(TKey id);
}
