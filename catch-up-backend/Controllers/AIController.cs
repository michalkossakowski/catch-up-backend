using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;
        public AIController(IAIService aiService) 
        {
            _aiService = aiService;
        }

        [HttpPost]
        [Route("GetAIChatResponse")]
        public async Task<IActionResult> GetAIChatResponse([FromBody] AIChatDto aiChatDto)
        {
            var result = await _aiService.GenerateAIChatResponse(aiChatDto);

            return string.IsNullOrEmpty(result)
                ? StatusCode(500, new { message = "Chat response error, try again later." })
                : Ok(new { message = result });
        }
    }
}
