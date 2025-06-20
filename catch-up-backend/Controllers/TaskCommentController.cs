﻿using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Helpers;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using catch_up_backend.Models;


namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskCommentController : ControllerBase
    {
        private readonly ITaskCommentService _taskCommentService;
        public TaskCommentController(ITaskCommentService taskCommentService)
        {
            _taskCommentService = taskCommentService;
        }
        [HttpPost]
        [Route("AddTaskComment")]
        public async Task<IActionResult> AddTaskComment([FromBody] TaskCommentDto newTaskComment)
        {
            
            try
            {
                var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
                newTaskComment.CreatorId = userId;
                var result = await _taskCommentService.AddAsync(newTaskComment);
                return result != null
                    ? Ok(new { message = "Task comment added", taskComment = result })
                    : StatusCode(500, new { message = "Error: Task comment add" });
            }
            catch(Exception ex)
            {
                return BadRequest($"Task comment add error: {ex}");
            }
            
        }
        [HttpPut]
        [Route("EditTaskComment/{commentId:int}")]
        public async Task<IActionResult> EditTaskComment(int commentId, TaskCommentDto newTaskComment)
        {
            newTaskComment = await _taskCommentService.EditAsync(commentId, newTaskComment);
            return newTaskComment != null
                ? Ok(new { message = $"Task comment edited", taskComment = newTaskComment })
                : StatusCode(500, new { message = "Task comment editing error", commentId = commentId });
        }
        [HttpPatch]
        [Route("PatchTaskComment/{commentId:int}")]
        public async Task<IActionResult> EditTaskCommentPatch(int commentId, [FromBody] JsonPatchDocument<TaskCommentModel> patchDoc)
        {
            var newTaskComment = await _taskCommentService.PatchAsnc(commentId, patchDoc);
            return newTaskComment != null
                ? Ok(new { message = $"Task comment edited", taskComment = newTaskComment })
                : StatusCode(500, new { message = "Task comment editing error", commentId = commentId });
        }

        [HttpDelete]
        [Route("DeleteTaskComment/{commentId:int}")]
        public async Task<IActionResult> DeleteTaskComment(int commentId)
        {
            return await _taskCommentService.DeleteAsync(commentId)
                ? Ok(new { message = $"Task comment deleted", commentId = commentId })
                : StatusCode(500, new { message = "Task comment deleting error", commentId = commentId });
        }
        [HttpGet]
        [Route("GetAllTaskComments")]
        public async Task<IActionResult> GetAllTaskComments()
        {
            var taskComments = await _taskCommentService.GetAllAsync();
            return Ok(taskComments);
        }
        [HttpGet]
        [Route("GetTaskCommentById/{commentId:int}")]
        public async Task<IActionResult> GetTaskCommentById(int commentId)
        {
            var taskComment = await _taskCommentService.GetByIdAsync(commentId);
            if (taskComment == null)
                return NotFound(new { message = $"There is no task comment with ID: [{commentId}]" });
            return Ok(taskComment);
        }
        [HttpGet]
        [Route("GetTaskCommentsByTaskId/{taskId:int}/{page:int}/{pagesize:int}")]
        public async Task<IActionResult> GetTaskCommentsByTaskId(int taskId, int page = 1, int pagesize = 5)
        {
            var result = await _taskCommentService.GetByTaskIdAsync(taskId, page, pagesize);
            if (!result.comments.Any())
                return NotFound(new { message = $"There is no task comments with Task ID: [{taskId}]" });
            return Ok(new { result.comments, result.totalCount });
        }
    }

}
