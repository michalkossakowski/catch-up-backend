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

        public FaqService(CatchUpDbContext contex)
        {
            _context = contex;
        }
        public async Task<bool> Add(FaqDto newQuestion)
        {
            try
            {
                var question = new FaqModel(
                newQuestion.Title ?? "",
                newQuestion.Answer ?? "",
                newQuestion.MaterialsId);
                await _context.AddAsync(question);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw new Exception("Error: Faq Add " + e);
            }
            return true;
        }

        public async Task<bool> Edit(int questionId, FaqDto newQuestion)
        {
            var question = await _context.Faqs.FindAsync(questionId);
            if (question == null)
                return false;
            try
            {
                question.Title = newQuestion.Title ?? "";
                question.Answer = newQuestion.Answer ?? "";
                question.MaterialsId = newQuestion.MaterialsId;
                _context.Faqs.Update(question);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) 
            {
                throw new Exception("Error: Faq Edit " + e);
            }
            return true;
        }
        public async Task<bool> Delete(int questionId)
        {
            var question = await _context.Faqs.FindAsync(questionId);
            if (question == null)
                return false;
            try
            {
                question.State = StateEnum.Deleted;
                _context.Faqs.Update(question);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: Faq Edit " + e);
            }
            return true;
        }
        public async Task<FaqDto> GetById(int questionId)
        {
            var question = await _context.Faqs
                .Where(q => q.State == StateEnum.Active && q.Id == questionId)
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
                .Where(q => q.State == StateEnum.Active)
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
                .Where(q => q.State == StateEnum.Active && q.Title.ToLower().Contains(title.ToLower()))
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
