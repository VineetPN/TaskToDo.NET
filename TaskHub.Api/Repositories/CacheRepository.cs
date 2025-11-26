
namespace TaskHub.Api.Repositories;
using TaskHubCore.Core;
using TaskHubCore.Core.Repositories;
using TaskHubCore.Interfaces;

public class CacheRepository<TKey, TEntity> : ICacheRepository<TKey, TEntity> where TEntity : IEntity<TKey>
{
    private readonly IRepositoryEntity<TKey, TEntity> _entity;
    public CacheRepository(IRepositoryEntity<TKey, TEntity> repository)
    {
        _entity = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool?> CreateTask(TKey key, TEntity entity)
    {
        try{
            await _entity.AddAsync(key, entity);
            return true;
        }
        catch(Exception){
            throw;
        }
    }

    public async Task<bool?> DeleteAllTasksAsync()
    {
        try{
            return await _entity.DeleteAllAsync();
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<bool?> DeleteTaskAsync(TKey guid)
    {
        try{
            return await _entity.DeleteAsync(guid);
        }
        catch(Exception){
            throw;
        }
    }

    public async Task<IAsyncEnumerable<TEntity>> GetAllTasksAsync() => _entity.GetAllAsync() ?? throw new ArgumentNullException(nameof(_entity));

    public async Task<TEntity> GetTaskAsync(TKey guid)
    {
        return await  _entity.GetAsync(guid) ?? throw new ArgumentNullException(nameof(guid));
    }

    public async Task<bool> UpdateTaskAsync(TKey key, TEntity NewTaskDetails) => await _entity.UpdateAsync(key, NewTaskDetails);
}