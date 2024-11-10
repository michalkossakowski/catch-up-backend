using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class FaqService : IFaqService
    {
        private readonly CatchUpDbContext _context;

        public FaqService(CatchUpDbContext contex)
        {
            _context = contex;
        }
        public async Task Add(FaqDto newQuestion)
        {
            var question = new FaqModel(
                newQuestion.Title,
                newQuestion.Answer,
                newQuestion.MaterialsId);
            await _context.AddAsync(question);
            await _context.SaveChangesAsync();
        }
        public async Task Edit(int questionId, FaqDto newQuestion)
        {
            var question = await _context.Faqs.FindAsync(questionId);
            question.Title = newQuestion.Title;
            question.Answer = newQuestion.Answer;
            question.MaterialsId = newQuestion.MaterialsId;
            _context.Faqs.Update(question);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int questionId)
        {
            var question = await _context.Faqs.FindAsync(questionId);
            _context.Faqs.Remove(question);
            await _context.SaveChangesAsync();
        }
        public async Task<FaqDto> GetById(int questionId)
        {
            var question = await _context.Faqs
                .Where(q => q.Id == questionId)
                .Select(q => new FaqDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Answer = q.Answer,
                    MaterialsId = q.MaterialsId
                }).FirstOrDefaultAsync();

            return question;
        }
        public async Task<List<FaqDto>> GetAll()
        {
            var questions = await _context.Faqs
                .Select(q => new FaqDto
                { 
                   Id = q.Id,
                   Title = q.Title,
                   Answer = q.Answer,
                   MaterialsId = q.MaterialsId
                })
               .ToListAsync();

            return questions;
        }
        public async Task<List<FaqDto>> GetByTitle(string title)
        {
            var questions = await _context.Faqs
                .Where(q => q.Title.ToLower().Contains(title.ToLower()))
                .Select(q => new FaqDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Answer = q.Answer,
                    MaterialsId = q.MaterialsId
                })
               .ToListAsync();

            return questions;
        }
    }
}
