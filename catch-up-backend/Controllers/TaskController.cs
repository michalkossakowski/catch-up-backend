using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
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
        [Route("AddTaskToUser")]
        public async Task<IActionResult> AddTaskToUser([FromBody] TaskDto newTask)
        {
            var result = await _taskService.Add(newTask);
            return result != null
                ? Ok(new { message = "Task added", task = result })
                : StatusCode(500, new { message = "Error: Task add" });
        }
        [HttpPut]
        [Route("EditTask/{taskId:int}")]
        public async Task<IActionResult> Edit(int taskId, TaskDto newTask)
        {

            return await _taskService.Edit(taskId, newTask)
                ? Ok(new { message = $"Task edited", task = newTask })
                : StatusCode(500, new { message = "Task editing error", taskId = taskId });
        }
        [HttpPut]
        [Route("EditFullTaskAsync/{taskId:int}/{userId:guid}")]
        public async Task<IActionResult> EditFullTask(int taskId, FullTask fullTask,Guid userId)
        {
            var (task, taskContent) = await _taskService.EditFullTaskAsync(taskId, fullTask, userId);
            if (task == null || taskContent == null)
                return NotFound(new { message = $"Task with id: [{taskId}] not found" , fullTaskId = taskId });
            return Ok(new { message = $"FullTask edited", fullTask = fullTask, task = task, taskContent = taskContent });
        }
        [HttpGet]
        [Route("GetAllTasksAsync")]
        public async Task<IActionResult> GetAllTasks()
        {
            var fullTasks = await _taskService.GetAllTasksAsync();
            return Ok(fullTasks);
        }
        [HttpGet]
        [Route("GetAllTasksByAssigningId/{AssigningId:guid}")]
        public async Task<IActionResult> GetAllTasksByAssigningId(Guid AssigningId)
        {
            var fullTasks = await _taskService.GetAllTasksByNewbieIdAsync(AssigningId);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no user with ID: [{AssigningId}]" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllTasksByNewbieIdAsync/{newbieID:guid}")]
        public async Task<IActionResult> GetAllTasksByNewbieId(Guid newbieID)
        {
            var fullTasks = await _taskService.GetAllTasksByNewbieIdAsync(newbieID);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no user with ID: [{newbieID}]" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllTaskByTaskContentIdAsync/{taskContentId:int}")]
        public async Task<IActionResult> GetAllTaskByTaskContentId(int taskContentId)
        {
            var fullTasks = await _taskService.GetAllTaskByTaskContentIdAsync(taskContentId);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no TaskContent with ID: [{taskContentId}]" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetTaskByIdAsync/{id:int}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var fullTask = await _taskService.GetTaskByIdAsync(id);
            if (fullTask == null)
                return NotFound(new { message = $"Task with id: [{id}] not found" });
            return Ok(fullTask);
        }

        [HttpGet]
        [Route("GetAllFullTasksAsync")]
        public async Task<IActionResult> GetAllFullTasks()
        {
            var fullTasks = await _taskService.GetAllFullTasksAsync();
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTasksByNewbieIdAsync/{newbieID:guid}")]
        public async Task<IActionResult> GetAllFullTasksByNewbieId(Guid newbieID)
        {
            var fullTasks = await _taskService.GetAllFullTasksByNewbieIdAsync(newbieID);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no user with ID: [{newbieID}]" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTaskByTaskContentIdAsync/{taskContentId:int}")]
        public async Task<IActionResult> GetAllFullTaskByTaskContentId(int taskContentId)
        {
            var fullTasks = await _taskService.GetAllFullTaskByTaskContentIdAsync(taskContentId);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no TaskContent with ID: [{taskContentId}]" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTasksByAssigningIdAsync/{AssigningId:guid}")]
        public async Task<IActionResult> GetAllFullTasksByAssigningId(Guid AssigningId)
        {
            var fullTasks = await _taskService.GetAllFullTasksByAssigningIdAsync(AssigningId);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no user with ID: [{AssigningId}]" });
            return Ok(fullTasks);
        }
        
        [HttpGet]
        [Route("GetFullTaskByIdAsync/{taskId:int}")]
        public async Task<IActionResult> GetFullTaskById(int taskId)
        {
            var fullTask = await _taskService.GetFullTaskByIdAsync(taskId);
            if (fullTask == null)
                return NotFound(new { message = $"Task with id: [{taskId}] not found" });
            return Ok(fullTask);
        }
        [HttpDelete]
        [Route("Delete/{taskId:int}")]
        public async Task<IActionResult> Delete(int taskId)
        {
            return await _taskService.Delete(taskId)
                ? Ok(new { message = "Task deleted successfully" })
                : NotFound(new { message = "Task not found." });
        }
    }
}
