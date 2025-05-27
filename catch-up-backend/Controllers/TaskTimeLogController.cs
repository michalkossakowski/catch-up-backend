using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskTimeLogController : ControllerBase
    {
        private readonly ITaskTimeLogService _taskTimeLogService;
        private readonly ICompanySettingsService _companySettingsService;
        public TaskTimeLogController(ITaskTimeLogService taskTimeLogService, ICompanySettingsService companySettingsService)
        {
            _taskTimeLogService = taskTimeLogService;
            _companySettingsService = companySettingsService;
        }
        [HttpPost]
        [Route("AddTaskTimeLog")]
        public async Task<IActionResult> AddTaskTimeLog([FromBody] TaskTimeLogDto newTaskTimeLog)
        {
            var settings = await _companySettingsService.GetCompanySettings();
            if(settings.Settings.ContainsKey("EnableTaskTimeLog") && settings.Settings["EnableTaskTimeLog"] == false)
            {
                return BadRequest(new { message = "Task time log feature is disabled" });
            }
            var result = await _taskTimeLogService.AddAsync(newTaskTimeLog);
            return result != null
                ? Ok(new { message = "Task time log added", taskTimeLog = result })
                : StatusCode(500, new { message = "Error: Task time log add" });
        }
        [HttpPut]
        [Route("EditTaskTimeLog/{taskTimeLogId:int}")]
        public async Task<IActionResult> EditTaskTaskTimeLog(int taskTimeLogId, TaskTimeLogDto newTaskTimeLog)
        {
            var settings = await _companySettingsService.GetCompanySettings();
            if (settings.Settings.ContainsKey("EnableTaskTimeLog") && settings.Settings["EnableTaskTimeLog"] == false)
            {
                return BadRequest(new { message = "Task time log feature is disabled" });
            }
            newTaskTimeLog = await _taskTimeLogService.EditAsync(taskTimeLogId, newTaskTimeLog);
            return newTaskTimeLog != null
                ? Ok(new { message = $"Task time log edited", taskTimeLog = newTaskTimeLog })
                : StatusCode(500, new { message = "Task time log editing error", taskTimeLogId = taskTimeLogId });
        }
        [HttpDelete]
        [Route("DeleteTaskTimeLog/{taskTimeLogId:int}")]
        public async Task<IActionResult> DeleteTaskTaskTimeLog(int taskTimeLogId)
        {
            var settings = await _companySettingsService.GetCompanySettings();
            if (settings.Settings.ContainsKey("EnableTaskTimeLog") && settings.Settings["EnableTaskTimeLog"] == false)
            {
                return BadRequest(new { message = "Task time log feature is disabled" });
            }
            return await _taskTimeLogService.DeleteAsync(taskTimeLogId)
                ? Ok(new { message = $"Task time log deleted", taskTimeLogId = taskTimeLogId })
                : StatusCode(500, new { message = "Task time log deleting error", taskTimeLogId = taskTimeLogId });
        }
        [HttpGet]
        [Route("GetAllTaskTimeLogs")]
        public async Task<IActionResult> GetAllTaskTimeLogs()
        {
            var settings = await _companySettingsService.GetCompanySettings();
            if (settings.Settings.ContainsKey("EnableTaskTimeLog") && settings.Settings["EnableTaskTimeLog"] == false)
            {
                return BadRequest(new { message = "Task time log feature is disabled" });
            }
            var taskTimeLogs = await _taskTimeLogService.GetAllAsync();
            return Ok(taskTimeLogs);
        }
        [HttpGet]
        [Route("GetTaskTimeLogById/{taskTimeLogId:int}")]
        public async Task<IActionResult> GetTaskTimeLogById(int taskTimeLogId)
        {
            var settings = await _companySettingsService.GetCompanySettings();
            if (settings.Settings.ContainsKey("EnableTaskTimeLog") && settings.Settings["EnableTaskTimeLog"] == false)
            {
                return BadRequest(new { message = "Task time log feature is disabled" });
            }

            var taskTimeLog = await _taskTimeLogService.GetByIdAsync(taskTimeLogId);
            if (taskTimeLog == null)
                return NotFound(new { message = $"There is no task time log with ID: [{taskTimeLogId}]" });
            return Ok(taskTimeLog);
        }
        [HttpGet]
        [Route("GetTaskTimeLogByTaskId/{taskId:int}/{page:int}/{pageSize:int}")]
        public async Task<IActionResult> GetTaskTimeLogByTaskId(int taskId, int page = 1, int pageSize = 5)
        {
            var settings = await _companySettingsService.GetCompanySettings();
            if (settings.Settings.ContainsKey("EnableTaskTimeLog") && settings.Settings["EnableTaskTimeLog"] == false)
            {
                return BadRequest(new { message = "Task time log feature is disabled" });
            }
            var result = await _taskTimeLogService.GetByTaskIdAsync(taskId,page, pageSize);
            if (!result.timeLogs.Any() == null)
                return NotFound(new { message = $"There is no task time logs with ID: [{taskId}]" });
            return Ok(new { result.timeLogs, result.totalCount, result.hours, result.minutes });
        }
    }
}
