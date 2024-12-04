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
        [Route("AddTaskToUser")]
        public async Task<IActionResult> AddTaskToUser([FromBody] TaskDto newTask)
        {
            await _taskService.Add(newTask);
            return Ok(new { message = "Task added", task = newTask });
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
        [Route("EditFullTask/{taskId:int}/{userId:guid}")]
        public async Task<IActionResult> EditFullTask(int taskId, FullTask fullTask,Guid userId)
        {
            return await _taskService.EditFullTask(taskId, fullTask, userId)
                ? Ok(new { message = $"FullTask edited", fullTask = fullTask })
                : StatusCode(500, new { message = "FullTask editing error", fullTaskId = taskId });
        }
        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var fullTasks = await _taskService.GetAllTasks();
            if (!fullTasks.Any())
                return NotFound(new { message = "No tasks found" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllTasksByNewbieId/{newbieID:guid}")]
        public async Task<IActionResult> GetAllTasksByNewbieId(Guid newbieID)
        {
            var fullTasks = await _taskService.GetAllTasksByNewbieId(newbieID);
            if (!fullTasks.Any())
                return NotFound(new { message = "No tasks found" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllTaskByTaskContentId/{taskContentId:int}")]
        public async Task<IActionResult> GetAllTaskByTaskContentId(int taskContentId)
        {
            var fullTasks = await _taskService.GetAllTaskByTaskContentId(taskContentId);
            if (!fullTasks.Any())
                return NotFound(new { message = "No tasks found" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetTaskById/{id:int}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var fullTask = await _taskService.GetTaskById(id);
            if (fullTask == null)
                return NotFound(new { message = $"Task with id: [{id}] not found" });
            return Ok(fullTask);
        }

        [HttpGet]
        [Route("GetAllFullTasks")]
        public async Task<IActionResult> GetAllFullTasks()
        {
            var fullTasks = await _taskService.GetAllFullTasks();
            if(!fullTasks.Any())
                return NotFound(new { message = "No tasks found" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTasksByNewbieId/{newbieID:guid}")]
        public async Task<IActionResult> GetAllFullTasksByNewbieId(Guid newbieID)
        {
            var fullTasks = await _taskService.GetAllFullTasksByNewbieId(newbieID);
            if (!fullTasks.Any())
                return NotFound(new { message = "No tasks found" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTaskByTaskContentId/{taskContentId:int}")]
        public async Task<IActionResult> GetAllFullTaskByTaskContentId(int taskContentId)
        {
            var fullTasks = await _taskService.GetAllFullTaskByTaskContentId(taskContentId);
            if (!fullTasks.Any())
                return NotFound(new { message = "No tasks found" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTasksByCreatorId/{creatorID:guid}")]
        public async Task<IActionResult> GetAllFullTasksByCreatorId(Guid creatorID)
        {
            var fullTasks = await _taskService.GetAllFullTasksByCreatorId(creatorID);
            if (!fullTasks.Any())
                return NotFound(new { message = "No tasks found" });
            return Ok(fullTasks);
        }
        
        [HttpGet]
        [Route("GetFullTaskById/{id:int}")]
        public async Task<IActionResult> GetFullTaskById(int id)
        {
            var fullTask = await _taskService.GetFullTaskById(id);
            if (fullTask == null)
                return NotFound(new { message = $"Task with id: [{id}] not found" });
            return Ok(fullTask);
        }
    }
}
