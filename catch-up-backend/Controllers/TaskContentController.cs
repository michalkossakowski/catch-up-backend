using Microsoft.AspNetCore.Mvc;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;


namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskContentController : ControllerBase
    {
        private readonly ITaskContentService _taskContentService;
        public TaskContentController(ITaskContentService taskContentService)
        {
            _taskContentService = taskContentService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] TaskContentDto newTaskContent)
        {
            return await _taskContentService.Add(newTaskContent)
                ? Ok(new { message = "Task content added", taskContent = newTaskContent })
                : StatusCode(500, new { message = "Error: Task content add" });
        }

        [HttpPut]
        [Route("Edit/{taskContentId:int}")]
        public async Task<IActionResult> Edit(int taskContentId, [FromBody] TaskContentDto newTaskContent)
        {
            return await _taskContentService.Edit(taskContentId, newTaskContent)
                ? Ok(new { message = "Task content edited", taskContent = newTaskContent })
                : StatusCode(500, new { message = "Error: Task content edit" });
        }

        [HttpDelete]
        [Route("Delete/{taskContentId:int}")]
        public async Task<IActionResult> Delete(int taskContentId)
        {
            return await _taskContentService.Delete(taskContentId)
                ? Ok(new { message = "Task content deleted", taskContent = taskContentId })
                : NotFound(new { message = "Error: Task content delete", taskContent = taskContentId });
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var taskContent = await _taskContentService.GetAll();
            if (!taskContent.Any())
                return NotFound(new { message = "No task content found" });
            return Ok(taskContent);
        }

        [HttpGet]
        [Route("GetById/{taskContentId:int}")]
        public async Task<IActionResult> GetById(int taskContentId)
        {
            var taskContent = await _taskContentService.GetById(taskContentId);
            if (taskContent == null)
                return NotFound(new { message = $"Task content with id: {taskContentId} not found" });
            return Ok(taskContent);
        }

        [HttpGet]
        [Route("GetByTitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var taskContents = await _taskContentService.GetByTitle(title);
            return Ok(taskContents);
        }

        [HttpGet]
        [Route("GetByCreatorId/{creatorId:Guid}")]
        public async Task<IActionResult> GetByCreatorId(Guid creatorId)
        {
            var taskContent = await _taskContentService.GetByCreatorId(creatorId);
            if (!taskContent.Any())
                return NotFound(new { message = $"No task content found for creator with id: {creatorId}" });
            return Ok(taskContent);
        }

        [HttpGet]
        [Route("GetByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(int categoryId)
        {
            var taskContent = await _taskContentService.GetByCategoryId(categoryId);
            if (!taskContent.Any())
                return NotFound(new { message = $"No task content found for category with id: {categoryId}" });
            return Ok(taskContent);
        }
    }
}
