using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IFaqService
    {
        public Task<FaqDto> AddAsync(FaqDto newFaq);
        public Task<FaqDto> EditAsync(int faqId, FaqDto newFaq);
        public Task<bool> DeleteAsync(int faqId);
        public Task<FaqDto> GetByIdAsync(int faqId);
        public Task<List<FaqDto>> GetAllAsync();
        public Task<List<FaqDto>> GetByQuestionAsync(string searchingQuestion);
    }
}
