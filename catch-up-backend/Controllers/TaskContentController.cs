using Microsoft.AspNetCore.Mvc;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using catch_up_backend.Response;


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
            var result = await _taskContentService.Add(newTaskContent);
            return result != null
                ? Ok(new { message = "Task content added", taskContent = result })
                : StatusCode(500, new { message = "Error: Task content add" });
        }

        [HttpPut]
        [Route("Edit/{taskContentId:int}")]
        public async Task<IActionResult> Edit(int taskContentId, [FromBody] TaskContentDto newTaskContent)
        {
            var result = await _taskContentService.Edit(taskContentId, newTaskContent);
            return result != null
                ? Ok(new { message = "Task content edited", taskContent = result })
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
        [Route("GetAll/{page}/{pageSize}")]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 5)
        {
            var taskContent = await _taskContentService.GetAll(page, pageSize);
            if (!taskContent.taskContents.Any())
                return NotFound(new { message = "No task content found" });
            return Ok( new { taskContent.taskContents, taskContent.totalCount });
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<PagedResponse<TaskContentDto>>> GetTaskContents([FromQuery] TaskContentQueryParameters parameters)
        {
            if (parameters.PageNumber < 1 || parameters.PageSize < 1)
            {
                return BadRequest("PageNumber and PageSize must be greater than 0.");
            }

            var result = await _taskContentService.GetTaskContentsAsync(parameters);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetById/{taskContentId:int}")]
        public async Task<IActionResult> GetById(int taskContentId)
        {
            try
            {
                var taskContent = await _taskContentService.GetById(taskContentId);
                if (taskContent == null)
                    return NotFound(new { message = $"Task content with id: {taskContentId} not found" });
                return Ok(taskContent);
            }
            catch (Exception)
            {
                return NotFound(new { message = $"Task content with id: {taskContentId} not found" });
            }
        }

        [HttpGet]
        [Route("GetByTitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            try
            {
                var taskContents = await _taskContentService.GetByTitle(title);
                if (!taskContents.Any())
                    return NotFound(new { message = $"No task content found with title containing: {title}" });
                return Ok(taskContents);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = $"Error retrieving task content with title: {title}" });
            }
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
