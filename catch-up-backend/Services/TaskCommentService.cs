using Azure.Core;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Helpers;
using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace catch_up_backend.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly IUserRepository _userRepository;
        private readonly CatchUpDbContext _context;
        public TaskCommentService(CatchUpDbContext context, IUserRepository userRepository)
        {
            _context = context;
            this._userRepository = userRepository;
        }

        public async Task<TaskCommentDto> AddAsync(TaskCommentDto newTaskComment)
        {
            try
            {
                var comment = new TaskCommentModel(newTaskComment);
                comment.CreationDate = DateTime.Now;
                comment.ModificationDate = null;
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
        public async Task<TaskCommentDto> PatchAsnc(int id, JsonPatchDocument<TaskCommentModel> patchDoc)
        {
            try
            {
                var comment = await _context.TaskComments.FindAsync(id);
                if (comment == null)
                {
                    return null;
                }
                var restrictedFields = new[] { "/id", "/taskid", "/creatorid", "/creationdate", "/modificationdate" };
                var allowedOperations = patchDoc.Operations
                    .Where(op => !restrictedFields.Contains(op.path.ToLower()))
                    .ToList();
                var filteredPatchDoc = new JsonPatchDocument<TaskCommentModel>(allowedOperations, patchDoc.ContractResolver);

                filteredPatchDoc.ApplyTo(comment);
                comment.ModificationDate = DateTime.Now;
                _context.TaskComments.Update(comment);
                await _context.SaveChangesAsync();
                return new TaskCommentDto(comment);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<TaskCommentDto>> GetAllAsync()
        {
            var comments = await _context.TaskComments
                .Where(x => x.State == StateEnum.Active)
                .Select(x => new TaskCommentDto(x)).ToListAsync();

            foreach (var comment in comments)
            {
                var user = await _userRepository.GetById(comment.CreatorId);
                comment.CreatorName = user.Name + " " + user.Surname;
            }
            return comments;
        }

        public async Task<TaskCommentDto> GetByIdAsync(int id)
        {
            var comment = await _context.TaskComments
                .Where(x => x.Id == id && x.State == StateEnum.Active)
                .Select(x => new TaskCommentDto(x)).FirstOrDefaultAsync();
            if (comment == null)
            {
                return null;
            }
            var user = await _userRepository.GetById(comment.CreatorId);
            comment.CreatorName = user.Name + " " + user.Surname;
            return comment;
        }

        public async Task<(List<TaskCommentDto> comments, int totalCount)> GetByTaskIdAsync(int taskId, int page, int pageSize)
        {
            var totalCount = await _context.TaskComments
                .Where(x => x.TaskId == taskId && x.State == StateEnum.Active)
                .CountAsync();
            var comments = await _context.TaskComments
                .Where(x => x.TaskId == taskId && x.State == StateEnum.Active)
                .OrderByDescending(x=>x.CreationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TaskCommentDto(x))
                .ToListAsync();
            foreach (var comment in comments)
            {
                var user = await _userRepository.GetById(comment.CreatorId);
                comment.CreatorName = user.Name + " " + user.Surname;
            }
            return (comments, totalCount);
        }
    }
}
