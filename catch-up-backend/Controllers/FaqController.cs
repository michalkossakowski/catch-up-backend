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
        public async Task<IActionResult> Add([FromBody] FaqDto newFaq)
        {
            var result = await _faqService.AddAsync(newFaq);

            return result != null
                ? Ok(new { message = "FAQ added", faq = result })
                : StatusCode(500, new { message = "FAQ adding error" });
        }

        [HttpPut]
        [Route("Edit/{faqId:int}")]
        public async Task<IActionResult> Edit(int faqId, [FromBody] FaqDto newFaq)
        {
            var result = await _faqService.EditAsync(faqId, newFaq);
            return result != null
                ? Ok(new { message = $"FAQ edited", faq = result })
                : StatusCode(500, new { message = "FAQ editing error" });
        }

        [HttpDelete]
        [Route("Delete/{faqId:int}")]
        public async Task<IActionResult> Delete(int faqId)
        {
            return await _faqService.DeleteAsync(faqId) 
                ? Ok(new { message = $"FAQ '{faqId}' deleted"})
                : NotFound(new { message = $"FAQ with id: '{faqId}' not found" });
        }

        [HttpGet]
        [Route("GetById/{faqId:int}")]
        public async Task<IActionResult> GetById(int faqId)
        {
            var faq = await _faqService.GetByIdAsync(faqId);
            if (faq == null)
                return NotFound(new { message = $"FAQ with id: '{faqId}' not found" });
            return Ok(faq);
        }

        [HttpGet]
        [Route("GetAll/{page}/{pageSize}")]
        public async Task<IActionResult> GetAll(int page = 1,  int pageSize = 5)
        {
            var result = await _faqService.GetAllAsync(page, pageSize);
            if (!result.faqs.Any())
                return NotFound(new { message = "No FAQs found" });
            return Ok(new { result.faqs, result.totalCount });
        }

        [HttpGet]
        [Route("GetByQuestion/{searchingQuestion}")]
        public async Task<IActionResult> GetByQuestion(string searchingQuestion)
        {
            var faqs = await _faqService.GetByQuestionAsync(searchingQuestion);
            if (!faqs.Any())
                return NotFound(new { message = $"No FAQs found with: '{searchingQuestion}' in question" });
            return Ok(faqs);
        }
    }
}
