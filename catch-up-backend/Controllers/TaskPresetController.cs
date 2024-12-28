using Microsoft.AspNetCore.Mvc;
using catch_up_backend.Dtos;
using catch_up_backend.Services.Interfaces;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskPresetController : ControllerBase
    {
        private readonly ITaskPresetService _taskPresetService;

        public TaskPresetController(ITaskPresetService taskPresetService)
        {
            _taskPresetService = taskPresetService;
        }

        [HttpPost]
        [Route("AddAsync")]
        public async Task<IActionResult> Add([FromBody] TaskPresetDto newTaskPreset)
        {
            var result = await _taskPresetService.Add(newTaskPreset);
            return result != null
                ? Ok(new { message = "Task preset added", taskPreset = result })
                : StatusCode(500, new { message = "Error: Task preset add" });
        }

        [HttpPut]
        [Route("EditAsync/{taskPresetId:int}")]
        public async Task<IActionResult> Edit(int taskPresetId, [FromBody] TaskPresetDto newTaskPreset)
        {
            return await _taskPresetService.Edit(taskPresetId, newTaskPreset)
                ? Ok(new { message = "Task preset edited", taskPreset = newTaskPreset })
                : NotFound(new { message = "Error: Task preset edit" });
        }

        [HttpDelete]
        [Route("DeleteByPreset/{presetId:int}")]
        public async Task<IActionResult> DeleteByPresetId(int presetId)
        {
            return await _taskPresetService.DeleteByPresetId(presetId)
                ? Ok(new { message = "Task presets deleted", presetId = presetId })
                : NotFound(new { message = "Error: Task presets delete" });
        }

        [HttpDelete]
        [Route("RemoveTaskFromPreset/{presetId:int}/{taskContentId:int}")]
        public async Task<IActionResult> RemoveTaskFromPreset(int presetId, int taskContentId)
        {
            return await _taskPresetService.RemoveTaskFromPreset(presetId, taskContentId)
                ? Ok(new { message = "Task removed from preset" })
                : NotFound(new { message = "Error: Task remove from preset" });
        }

        [HttpDelete]
        [Route("RemoveTaskFromAllPresets/{taskContentId:int}")]
        public async Task<IActionResult> RemoveTaskFromAllPresets(int taskContentId)
        {
            return await _taskPresetService.RemoveTaskFromAllPresets(taskContentId)
                ? Ok(new { message = "Task removed from all presets" })
                : NotFound(new { message = "Error: Task remove from all presets" });
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var taskPresets = await _taskPresetService.GetAll();
            if (!taskPresets.Any())
                return NotFound(new { message = "No task presets found" });
            return Ok(taskPresets);
        }

        [HttpGet]
        [Route("GetById/{taskPresetId:int}")]
        public async Task<IActionResult> GetById(int taskPresetId)
        {
            var taskPresets = await _taskPresetService.GetById(taskPresetId);
            if (!taskPresets.Any())
                return NotFound(new { message = $"No task presets found for id: {taskPresetId}" });
            return Ok(taskPresets);
        }

        [HttpGet]
        [Route("GetByPreset/{presetId:int}")]
        public async Task<IActionResult> GetByPreset(int presetId)
        {
            var taskPresets = await _taskPresetService.GetByPresetId(presetId);
            if (!taskPresets.Any())
                return NotFound(new { message = $"No task presets found for preset id: {presetId}" });
            return Ok(taskPresets);
        }

        [HttpGet]
        [Route("GetByTaskContent/{taskContentId:int}")]
        public async Task<IActionResult> GetByTaskContent(int taskContentId)
        {
            var taskPresets = await _taskPresetService.GetByTaskContent(taskContentId);
            if (!taskPresets.Any())
                return NotFound(new { message = $"No task presets found for task content id: {taskContentId}" });
            return Ok(taskPresets);
        }
    }
} 