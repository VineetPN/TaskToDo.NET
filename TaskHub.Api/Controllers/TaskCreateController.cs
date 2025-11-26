using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Api.Repositories;
using TaskHubCore.Core.DTO;
using TaskHubCore.Core.Repositories;
using TaskHubCore.Core.Tasks;

namespace TaskHub.Api.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class TaskCreateController : ControllerBase{

    private readonly ICacheRepository<Guid, TaskItem> _cacheRepo;
    private readonly ILogger<TaskCreateController> _logger;
    public TaskCreateController(ICacheRepository<Guid, TaskItem> cacheRepo, ILogger<TaskCreateController> logger)
    {
        _cacheRepo = cacheRepo ?? throw new ArgumentNullException(nameof(cacheRepo));
        _logger = logger;
    }
    [HttpPost]
    public async Task<IActionResult> CreateNewTask([FromBody]TaskCreateDTO taskCreateDTO){
        _logger.LogDebug($"CreateTaskAPI Called with name {taskCreateDTO.name}");
        try{
            if(taskCreateDTO == null || _cacheRepo == null) throw new Exception("Unable to update the data");
            var newTaskItem = new TaskItem(taskCreateDTO.name, taskCreateDTO.Description);
            var result = await _cacheRepo.CreateTask(newTaskItem.Id, newTaskItem);
            if(result.Value == true) return Ok("New Task Created successfully");
            else return BadRequest($"Unable to create a new task with guid {newTaskItem.Id}");
        }
        catch(Exception ex){
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }
}