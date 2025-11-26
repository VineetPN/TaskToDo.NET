using TaskHubCore.Interfaces;

namespace TaskHub.Api.Repositories;

public interface ICacheRepository<Tkey, TEntity> {
    Task<IAsyncEnumerable<TEntity>> GetAllTasksAsync();
    Task<TEntity> GetTaskAsync(Tkey guid);
    Task<bool?> DeleteTaskAsync(Tkey guid);
    Task<bool?> DeleteAllTasksAsync();
    Task<bool> UpdateTaskAsync(Tkey key, TEntity NewTaskDetails);
    Task<bool?> CreateTask(Tkey key, TEntity entity);
}