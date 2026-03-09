using Board.DataAccess.Models.Base;

namespace Board.DataAccess.Repository.Base;

public interface IEntityRepository<TKey, TEntity>
   where TEntity : class, IKeyedEntity<TKey>, new()
   where TKey : IEquatable<TKey>
{
    Task<TEntity> Create(TEntity entity);
    Task<TEntity> Update(TEntity entity);
    Task<TEntity?> GetById(TKey id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<bool> Exists(TKey id);
    Task<bool> Delete(TKey id);
}