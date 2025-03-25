using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace catch_up_backend.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly CatchUpDbContext _context;
        public TaskCommentService(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task<TaskCommentDto> AddAsync(TaskCommentDto newTaskComment)
        {
            try
            {
                var comment = new TaskCommentModel(newTaskComment);
                comment.CreationDate = DateTime.Now;
                comment.State = StateEnum.Active;
                _context.TaskComments.Add(comment);
                _context.SaveChanges();
                newTaskComment = new TaskCommentDto(comment);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return newTaskComment;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var comment = await _context.TaskComments.FindAsync(id);
                if (comment == null)
                {
                    return false;
                }
                comment.State = StateEnum.Deleted;
                _context.TaskComments.Update(comment);
                _context.SaveChanges();
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }

        public async Task<TaskCommentDto> EditAsync(int id, TaskCommentDto newTaskComment)
        {
            try
            {
                var comment = await _context.TaskComments.FindAsync(id);
                if (comment == null)
                {
                    return null;
                }
                comment.Content = newTaskComment.Content;
                comment.ModificationDate = DateTime.Now;
                comment.MaterialId = newTaskComment.MaterialId;
                _context.TaskComments.Update(comment);
                await _context.SaveChangesAsync();
                newTaskComment = new TaskCommentDto(comment);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return newTaskComment;
        }

        public async Task<List<TaskCommentDto>> GetAllAsync()
        {
            return await _context.TaskComments
                .Where(x => x.State == StateEnum.Active)
                .Select(x => new TaskCommentDto(x)).ToListAsync();
        }

        public async Task<TaskCommentDto> GetByIdAsync(int id)
        {
            return await _context.TaskComments
                .Where(x => x.Id == id && x.State == StateEnum.Active)
                .Select(x => new TaskCommentDto(x)).FirstOrDefaultAsync();
        }

        public async Task<List<TaskCommentDto>> GetByTaskIdAsync(int taskId)
        {
            return await _context.TaskComments
                .Where(x => x.TaskId == taskId && x.State == StateEnum.Active)
                .Select(x => new TaskCommentDto(x)).ToListAsync();
        }
    }
}
