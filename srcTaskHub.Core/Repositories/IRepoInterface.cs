using TaskHubCore.Interfaces;

namespace TaskHubCore.Core.Repositories;

public interface IRepositoryEntity<TKey, TEntity> where TEntity : IEntity<TKey>{
    ValueTask<TEntity?> GetAsync(TKey key);
    ValueTask AddAsync(TKey key, TEntity entity);
    ValueTask<bool> DeleteAsync(TKey key);
    ValueTask<bool> UpdateAsync(TKey key, TEntity entity);
    ValueTask<bool?> DeleteAllAsync();
    IAsyncEnumerable<TEntity> GetAllAsync();
}