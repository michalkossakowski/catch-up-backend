using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaqController : ControllerBase
    {
        private readonly IFaqService _faqService;
        public FaqController(IFaqService faqService) 
        { 
            _faqService = faqService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] FaqDto newQuestion)
        {
            return await _faqService.Add(newQuestion) 
                ? Ok(new { message = "FAQ added", faq = newQuestion })
                : StatusCode(500, new { message = "FAQ adding error"});
        }

        [HttpPut]
        [Route("Edit/{questionId:int}")]
        public async Task<IActionResult> Edit(int questionId, [FromBody] FaqDto newQuestion)
        {
            return await _faqService.Edit(questionId, newQuestion)
                ? Ok(new { message = $"FAQ edited", faq = newQuestion })
                : StatusCode(500, new { message = "FAQ editing error", faqId = questionId });
        }

        [HttpDelete]
        [Route("Delete/{questionId:int}")]
        public async Task<IActionResult> Delete(int questionId)
        {
            return await _faqService.Delete(questionId) 
                ? Ok(new { message = "FAQ deleted", faqId = questionId })
                : NotFound(new { message = "FAQ not found", faqId = questionId });
        }

        [HttpGet]
        [Route("GetById/{questionId:int}")]
        public async Task<IActionResult> GetById(int questionId)
        {
            var faq = await _faqService.GetById(questionId);
            if (faq == null)
                return NotFound(new { message = $"FAQ with id: [{questionId}] not found" });
            return Ok(faq);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var faqs = await _faqService.GetAll();
            if (!faqs.Any())
                return NotFound(new { message = "No FAQs found" });
            return Ok(faqs);
        }

        [HttpGet]
        [Route("GetByTitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var faqs = await _faqService.GetByTitle(title);
            if (!faqs.Any())
                return NotFound(new { message = $"No FAQs found with: '{title}' in title" });
            return Ok(faqs);
        }
    }
}
