using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Helpers;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using catch_up_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] FeedbackDto newFeedback)
        {
            return await _feedbackService.AddAsync(newFeedback)
                ? Ok(new { message = "Feedback added", feedback = newFeedback })
                : StatusCode(500, new { message = "Error: Feedback add" });
        }

        [HttpPut]
        [Route("Edit/{feedbackId:int}")]
        public async Task<IActionResult> Edit(int feedbackId, [FromBody] FeedbackDto newFeedback)
        {
            return await _feedbackService.EditAsync(feedbackId, newFeedback)
                ? Ok(new { message = "Feedback edited", feedback = newFeedback })
                : StatusCode(500, new { message = "Error: Feedback edit" });
        }

        [HttpDelete]
        [Route("Delete/{feedbackId:int}")]
        public async Task<IActionResult> Delete(int feedbackId)
        {
            return await _feedbackService.DeleteAsync(feedbackId)
                ? Ok(new { message = "Feedback deleted", feedback = feedbackId })
                : NotFound(new { message = "Error: Feedback delete", feedback = feedbackId });
        }

        [HttpGet]
        [Route("GetById/{feedbackId:int}")]
        public async Task<IActionResult> GetById(int feedbackId)
        {
            var feedback = await _feedbackService.GetByIdAsync(feedbackId);
            if (feedback == null)
                return NotFound(new { message = $"Feedback with id: {feedbackId} not found" });
            return Ok(feedback);
        }

        [HttpGet]
        [Route("GetByResource/{resourceType:int}/{resourceId:int}")]
        public async Task<IActionResult> GetByResource(ResourceTypeEnum resourceType, int resourceId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksByResourceAsync(resourceType, resourceId);

            return Ok(feedbacks);
        }

        [HttpGet]
        [Route("GetByTitle/{searchingTitle}")]
        public async Task<IActionResult> GetByTitle(string searchingTitle)
        {
            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
            var feedbacks = await _feedbackService.GetByTitleAsync(searchingTitle, userId);
            return Ok(feedbacks);
        }

        [HttpPut]
        [Route("ChangeDoneStatus/{feedbackId:int}")]
        public async Task<IActionResult> ChangeDoneStatus(int feedbackId)
        {
            return await _feedbackService.ChangeDoneStatusAsync(feedbackId)
                ? Ok(new { message = "Feedback status changed" })
                : StatusCode(500, new { message = "Error: Feedback status change" });
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
            var feedback = await _feedbackService.GetAllAsync(userId);
            return Ok(feedback);
        }
    }
}
