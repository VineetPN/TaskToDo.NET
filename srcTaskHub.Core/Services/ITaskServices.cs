using TaskHubCore.Core.DTO;
using TaskHubCore.Core.Tasks;
using TaskHubCore.Interfaces;

namespace TaskHubCore.Core.Services;

public interface ITaskService<TKey, TEntity> where TEntity : IEntity<TKey>{
    Task<TEntity> CreateTaskAsync(TaskCreateDTO taskCreateDTO, CancellationToken cancellationToken = default);
    Task<TEntity> GetTaskDetailsByID(Guid guid, CancellationToken cancellationToken = default);
    Task<IAsyncEnumerable<TaskItem>> GetAllTaskAsync(CancellationToken cancellationToken = default);
    Task<bool?> MakeTaskMarkDone(Guid id, CancellationToken cancellationToken = default);
}