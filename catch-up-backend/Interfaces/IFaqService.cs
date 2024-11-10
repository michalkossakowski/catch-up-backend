using catch_up_backend.Models;
using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IFaqService
    {
        public Task Add(FaqDto newQuestion);
        public Task Edit(int questionId, FaqDto newQuestion);
        public Task Delete(int questionId);
        public Task<FaqDto> GetById(int questionId);
        public Task<List<FaqDto>> GetAll();
        public Task<List<FaqDto>> GetByTitle(string searchingTitle);
    }
}
