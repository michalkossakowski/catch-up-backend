using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;

namespace catch_up_backend.Services
{
    public class FaqService : IFaqService
    {
        private readonly CatchUpDbContext _context;

        public FaqService(CatchUpDbContext context)
        {
            _context = context;
        }
        public async Task<FaqDto> AddAsync(FaqDto newFaq)
        {
            try
            {
                var question = new FaqModel(
                    newFaq.Question ?? "",
                    newFaq.Answer ?? "",
                    newFaq.MaterialId,
                    newFaq.CreatorId
                );
                await _context.AddAsync(question);
                await _context.SaveChangesAsync();
                newFaq.Id = question.Id;
            }
            catch(Exception ex)
            {
                throw new Exception("Error: Faq Add: " + ex);
            }
            return newFaq;
        }

        public async Task<FaqDto> EditAsync(int questionId, FaqDto newFaq)
        {
            var faq = await _context.Faqs.FindAsync(questionId);
            if (faq == null)
                return null;
            try
            {
                faq.Question = newFaq.Question ?? "";
                faq.Answer = newFaq.Answer ?? "";
                faq.MaterialId = newFaq.MaterialId;
                faq.CreatorId = newFaq.CreatorId;
                _context.Faqs.Update(faq);
                await _context.SaveChangesAsync();
                newFaq.Id = faq.Id;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error: Faq Edit: " + ex);
            }
            return newFaq;
        }
        public async Task<bool> DeleteAsync(int faqId)
        {
            var faq = await _context.Faqs.FindAsync(faqId);
            if (faq == null)
                return false;
            try
            {
                faq.State = StateEnum.Deleted;
                _context.Faqs.Update(faq);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Faq Delete: " + ex);
            }
            return true;
        }
        public async Task<FaqDto> GetByIdAsync(int questionId)
        {
            var faq = await _context.Faqs
                .Where(f => f.State == StateEnum.Active && f.Id == questionId)
                .Select(f => new FaqDto
                {
                    Id = f.Id,
                    Question = f.Question,
                    Answer = f.Answer,
                    MaterialId = f.MaterialId,
                    CreatorId = f.CreatorId
                }).FirstOrDefaultAsync();

            return faq;
        }
        public async Task<List<FaqDto>> GetAllAsync()
        {
            var faqs = await _context.Faqs
                .Where(f => f.State == StateEnum.Active)
                .Select(f => new FaqDto
                { 
                    Id = f.Id,
                    Question = f.Question,
                    Answer = f.Answer,
                    MaterialId = f.MaterialId,
                    CreatorId = f.CreatorId
                })
               .ToListAsync();

            return faqs;
        }
        public async Task<List<FaqDto>> GetByQuestionAsync(string title)
        {
            var faqs = await _context.Faqs
                .Where(f => f.State == StateEnum.Active && f.Question.ToLower().Contains(title.ToLower()))
                .Select(f => new FaqDto
                { 
                    Id = f.Id,
                    Question = f.Question,
                    Answer = f.Answer,
                    MaterialId = f.MaterialId,
                    CreatorId = f.CreatorId
                })
               .ToListAsync();

            return faqs;
        }
    }
}
