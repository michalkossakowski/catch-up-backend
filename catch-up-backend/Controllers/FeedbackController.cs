using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
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
            return await _feedbackService.Add(newFeedback)
                ? Ok(new { message = "Feedback added", feedback = newFeedback })
                : StatusCode(500, new { message = "Error: Feedback add" });
        }

        [HttpPut]
        [Route("Edit/{feedbackId:int}")]
        public async Task<IActionResult> Edit(int feedbackId, [FromBody] FeedbackDto newFeedback)
        {
            return await _feedbackService.Edit(feedbackId, newFeedback)
                ? Ok(new { message = "Feedback edited", badge = newFeedback })
                : StatusCode(500, new { message = "Error: Feedback edit" });
        }

        [HttpDelete]
        [Route("Delete/{feedbackId:int}")]
        public async Task<IActionResult> Delete(int feedbackId)
        {
            return await _feedbackService.Delete(feedbackId)
                ? Ok(new { message = "Feedback deleted", feedback = feedbackId })
                : NotFound(new { message = "Error: Feedback delete", feedback = feedbackId });
        }

        [HttpGet]
        [Route("GetById/{feedbackId:int}")]
        public async Task<IActionResult> GetById(int feedbackId)
        {
            var feedback = await _feedbackService.GetById(feedbackId);
            if (feedback == null)
                return NotFound(new { message = $"Feedback with id: {feedbackId} not found" });
            return Ok(feedback);
        }

        [HttpGet]
        [Route("GetBySenderId/{SenderId:string}")]
        public async Task<IActionResult> GetBySenderId(string senderId)
        {
            var feedback = await _feedbackService.GetBySenderId(senderId);
            if (feedback == null)
                return NotFound(new { message = $"Feedback with sender id: {senderId} not found" });
            return Ok(feedback);
        }

        [HttpGet]
        [Route("GetByReceiverId/{ReceiverId:string}")]
        public async Task<IActionResult> GetByReceiverId(string receiverId)
        {
            var feedback = await _feedbackService.GetByReceiverId(receiverId);
            if (feedback == null)
                return NotFound(new { message = $"Feedback with receiver id: {receiverId} not found" });
            return Ok(feedback);
        }

        [HttpGet]
        [Route("GetByOrigin/{Origin:string}")]
        public async Task<IActionResult> GetByOrigin(string origin)
        {
            var feedback = await _feedbackService.GetByOrigin(origin);
            if (feedback == null)
                return NotFound(new { message = $"Feedback with origin: {origin} not found" });
            return Ok(feedback);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var feedback = await _feedbackService.GetAll();
            if (!feedback.Any())
                return NotFound(new { message = "No feedbacks found" });
            return Ok(feedback);
        }
    }
}
