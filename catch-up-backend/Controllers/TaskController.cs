using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Route("AddTaskToUser/{newbieID:guid}/{taskContentId:int}")]
        public async Task<IActionResult> AddTaskToUser([FromBody] TaskDto newTask,Guid newbieID, int taskContentId)
        {
            await _taskService.Add(newTask, newbieID, taskContentId);
            return Ok(new { message = "Task added", task = newTask });
        }
        [HttpGet]
        [Route("GetAllFullTasks")]
        public async Task<List<FullTask>> GetAllFullTasks()
        {
            return await _taskService.GetAllTasks();
        }
    }
}
