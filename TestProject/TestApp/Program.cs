namespace TestAPP;

using Microsoft.Extensions.Logging.Abstractions;
using TaskHubCore.Core.DTO;
using TaskHubCore.Core.Repositories;
using TaskHubCore.Core.Services;
using TaskHubCore.Core.Tasks;


public class Tests{
    public async static Task Main(string[] args)
    {
        var logger = NullLogger<InMemoryRepository<Guid, TaskItem>>.Instance;
        var logger1 = NullLogger<InMemoryTaskService<Guid, TaskItem>>.Instance;
        IRepositoryEntity<Guid, TaskItem> repo = new InMemoryRepository<Guid, TaskItem>(logger);
        var task = new InMemoryTaskService<Guid, TaskItem>(repo, logger1);
        var taskDTO = new TaskCreateDTO("Check BP", "You BP is high");
        await task.CreateTaskAsync(taskDTO);
    }
}