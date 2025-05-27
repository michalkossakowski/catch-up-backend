using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITaskCommentService _taskCommentService;
        private readonly ITaskTimeLogService _taskTimeLogService;
        private readonly ICompanySettingsService _companySettingsService;
        public TaskController(ITaskService taskService, ITaskCommentService taskCommentService, ITaskTimeLogService taskTimeLogService, ICompanySettingsService companySettingsService)
        {
            _taskService = taskService;
            _taskCommentService = taskCommentService;
            _taskTimeLogService = taskTimeLogService;
            _companySettingsService = companySettingsService;
        }

        [HttpPost]
        [Route("AddTaskToUser")]
        public async Task<IActionResult> AddTaskToUser([FromBody] TaskDto newTask)
        {
            var result = await _taskService.AddAsync(newTask);

            if(result != null)
            {
                var newFullTask = await _taskService.GetFullTaskByIdAsync(result.Id);
                return Ok(new { message = "Task added", task = result, fullTask = newFullTask });
            }
                
            return StatusCode(500, new { message = "Error: Task add" });
        }

        [HttpPut]
        [Route("EditTask/{taskId:int}")]
        public async Task<IActionResult> Edit(int taskId, TaskDto newTask)
        {

            return await _taskService.EditAsync(taskId, newTask)
                ? Ok(new { message = $"Task edited", task = newTask })
                : StatusCode(500, new { message = "Task editing error", taskId = taskId });
        }
        [HttpPut]
        [Route("EditFullTask/{taskId:int}/{userId:guid}")]
        public async Task<IActionResult> EditFullTask(int taskId, FullTask fullTask,Guid userId)
        {
            var (task, taskContent) = await _taskService.EditFullTaskAsync(taskId, fullTask, userId);
            fullTask.Id = taskId;
            if (task == null || taskContent == null)
                return NotFound(new { message = $"Task with id: [{taskId}] not found" , fullTaskId = taskId });
            return Ok(new { message = $"FullTask edited", fullTask = fullTask, task = task, taskContent = taskContent });
        }
        [HttpGet]
        [Route("GetAllTasks")]
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
        [Route("GetAllTasksByNewbieId/{newbieID:guid}")]
        public async Task<IActionResult> GetAllTasksByNewbieId(Guid newbieID)
        {
            var fullTasks = await _taskService.GetAllTasksByNewbieIdAsync(newbieID);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no user with ID: [{newbieID}]" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllTaskByTaskContentId/{taskContentId:int}")]
        public async Task<IActionResult> GetAllTaskByTaskContentId(int taskContentId)
        {
            var fullTasks = await _taskService.GetAllTaskByTaskContentIdAsync(taskContentId);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no TaskContent with ID: [{taskContentId}]" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetTaskById/{id:int}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var fullTask = await _taskService.GetTaskByIdAsync(id);
            if (fullTask == null)
                return NotFound(new { message = $"Task with id: [{id}] not found" });
            return Ok(fullTask);
        }

        [HttpGet]
        [Route("GetAllFullTasks")]
        public async Task<IActionResult> GetAllFullTasks()
        {
            var fullTasks = await _taskService.GetAllFullTasksAsync();
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTasksByNewbieId/{newbieID:guid}")]
        public async Task<IActionResult> GetAllFullTasksByNewbieId(Guid newbieID)
        {
            var fullTasks = await _taskService.GetAllFullTasksByNewbieIdAsync(newbieID);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no user with ID: [{newbieID}]" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTasksByRoadMapPointId/{roadMapId:int}")]
        public async Task<IActionResult> GetAllFullTasksByRoadMapPointId(int roadMapId)
        {
            var fullTasks = await _taskService.GetAllFullTasksByRoadMapPointIdAsync(roadMapId);
            if (fullTasks == null || !fullTasks.Any())
                return Ok(new List<FullTask>());
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTaskByTaskContentId/{taskContentId:int}")]
        public async Task<IActionResult> GetAllFullTaskByTaskContentId(int taskContentId)
        {
            var fullTasks = await _taskService.GetAllFullTaskByTaskContentIdAsync(taskContentId);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no TaskContent with ID: [{taskContentId}]" });
            return Ok(fullTasks);
        }

        [HttpGet]
        [Route("GetAllFullTasksByAssigningId/{AssigningId:guid}")]
        public async Task<IActionResult> GetAllFullTasksByAssigningId(Guid AssigningId)
        {
            var fullTasks = await _taskService.GetAllFullTasksByAssigningIdAsync(AssigningId);
            if (fullTasks == null)
                return NotFound(new { message = $"There is no user with ID: [{AssigningId}]" });
            return Ok(fullTasks);
        }
        
        [HttpGet]
        [Route("GetFullTaskById/{taskId:int}")]
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
            return await _taskService.DeleteAsync(taskId)
                ? Ok(new { message = "Task deleted successfully" })
                : NotFound(new { message = "Task not found." });
        }

        [HttpPatch]
        [Route("SetStatus/{taskId:int}/{status:int}")]
        public async Task<IActionResult> SetStatusAsync(int taskId, int status)
        {
            return await _taskService.SetStatusAsync(taskId, (StatusEnum)status)
                ? Ok(new { message = $"Task status setted to {status}", task = taskId })
                : StatusCode(500, new { message = "Task status set error", task = taskId });
        }
        [HttpPatch]
        [Route("AddTime/{taskId:int}/{time:double}")]
        public async Task<IActionResult> AddTimeAsync(int taskId, double time)
        {
            var task = await _taskService.AddTimeAsync(taskId, time);
            return task != null
                ? Ok(new { message = $"Task time added. Current time: {task.SpendTime}", task = task })
                : StatusCode(500, new { message = "Task status adding error", taskId = taskId });
        }
        [HttpPatch]
        [Route("SetTime/{taskId:int}/{time:double}")]
        public async Task<IActionResult> SetTimeAsync(int taskId, double time)
        {
            if (time < 0)
                return StatusCode(400, new { message = "Time can't be negative", taskId = taskId });
            var task = await _taskService.SetTimeAsync(taskId, time);
            return task != null
                ? Ok(new { message = $"Task time setted to {task.SpendTime}", task = task })
                : StatusCode(500, new { message = "Task status set error", taskId = taskId });
        }
        [HttpPatch]
        [Route("SetRate/{taskId:int}/{rate:int}")]
        public async Task<IActionResult> SetTimeAsync(int taskId, int rate)
        {
            if (rate < 0)
                return StatusCode(400, new { message = "Rate can't be negative", taskId = taskId });
            var task = await _taskService.SetRateAsync(taskId, rate);
            return task != null
                ? Ok(new { message = $"Task rate setted to {task.SpendTime}", task = task })
                : StatusCode(500, new { message = "Task rate set error", taskId = taskId });
        }
        [HttpGet]
        [Route("GetFullTaskData/{taskId:int}")]
        public async Task<IActionResult> GetFullTaskData(int taskId)
        {
            var fullTaskResult = await _taskService.GetFullTaskByIdAsync(taskId);
            if (fullTaskResult == null)
                return NotFound(new { message = $"Task with id: [{taskId}] not found" });
            var commentResult = await _taskCommentService.GetByTaskIdAsync(taskId,1,5);
            
            var settings = await _companySettingsService.GetCompanySettings();
            var isTimeLogEnabled = settings.Settings.ContainsKey("EnableTaskTimeLog") && settings.Settings["EnableTaskTimeLog"] == true;
            if (!isTimeLogEnabled)
            {
                return Ok(new { fullTaskResult, commentResult.comments, commentResult.totalCount, isTimeLogEnabled });
            }
            var timeLogResult = await _taskTimeLogService.GetByTaskIdAsync(taskId, 1, 5);

            return Ok(new {fullTask = fullTaskResult, comments = commentResult.comments, commentsTotalCount = commentResult.totalCount, timelogs = timeLogResult.timeLogs, timeLogTotalCount = timeLogResult.totalCount, hours = timeLogResult.hours, minutes = timeLogResult.minutes, isTimeLogEnabled });
        }
    }
}
