using System.Formats.Tar;
using Microsoft.Extensions.Logging;
using TaskHubCore.Core.DTO;
using TaskHubCore.Core.Repositories;
using TaskHubCore.Core.Tasks;
using TaskHubCore.Interfaces;

namespace TaskHubCore.Core.Services;

public class InMemoryTaskService<TKey, TEntity> : ITaskService<TKey, TEntity> 
            where TEntity : IEntity<TKey>
{
    private readonly ILogger<InMemoryTaskService<TKey, TEntity>> _logger;
    private readonly IRepositoryEntity<Guid, TaskItem> _entityRepository;
    public InMemoryTaskService(IRepositoryEntity<Guid, TaskItem> repositoryEntity
        , ILogger<InMemoryTaskService<TKey, TEntity>> iLogger){

            _entityRepository = repositoryEntity;
            _logger = iLogger;
    
    }

    public async Task<TEntity?> CreateTaskAsync(TaskCreateDTO taskCreateDTO, CancellationToken cancellationToken = default)
    {
        //Create a task so the HTTPS request comes here so here the function can be called directly
        //we have the DTO here we will extract the info and then conver to a task object and then 
        //we will create a record using iRepo interface
        try{
            if (taskCreateDTO is null) throw new ArgumentNullException(nameof(taskCreateDTO));
            TaskItem taskItem = new TaskItem(taskCreateDTO.name, taskCreateDTO.Description);
            await _entityRepository.AddAsync(taskItem.Id, taskItem);
            return (TEntity)(object)taskItem;
        }
        catch(Exception ex){
            _logger.LogError(ex, "some exception");
            return (TEntity?)(object)null;
        }
        
    }

    public async Task<IAsyncEnumerable<TaskItem>> GetAllTaskAsync(CancellationToken cancellationToken = default)
    {
        try{
            return _entityRepository.GetAllAsync();
        }
        catch(Exception ex){
            _logger.LogError(ex, "Unable to get all tasks");
            return null;           
        }
    }

    public async Task<TEntity> GetTaskDetailsByID(Guid guid, CancellationToken cancellationToken = default)
    {
        try
        {
            return (TEntity)(object)await _entityRepository.GetAsync(guid);
        }
        catch(Exception ex){
            _logger.LogError(ex, $"Failed to get record for {guid}");
            return (TEntity?)(object)null;
        }
    }

    public async Task<bool?> MakeTaskMarkDone(Guid id, CancellationToken cancellationToken = default)
    {
        try{
            var TaskItem = _entityRepository.GetAsync(id).Result;
            var doneTaskItem = TaskItem?.Markdone() ?? throw new ArgumentNullException(nameof(TaskItem)); 
            return await _entityRepository.UpdateAsync(id, doneTaskItem);
            
        }
        catch(Exception ex){
            _logger.LogError(ex, "Unable to update as MarkDone");
            return false;
        }
    }
}