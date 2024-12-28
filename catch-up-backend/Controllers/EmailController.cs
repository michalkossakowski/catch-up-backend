using Microsoft.AspNetCore.Mvc;
using catch_up_backend.Services;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> SendEmail([FromQuery] string recipientEmail,[FromQuery] string subject,[FromQuery] string body,[FromQuery] List<string> attachments = null)
        {
            try
            {
                await _emailService.SendEmail(recipientEmail, subject, body, attachments);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Problem during sending mail: {ex.Message}");
            }
        }
    }
}
