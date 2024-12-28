using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;

namespace catch_up_backend.Services
{
    public class TaskContentService : ITaskContentService
    {
        private readonly CatchUpDbContext _context;

        public TaskContentService(CatchUpDbContext context)
        {   
            _context = context;
        }

        public async Task<TaskContentDto> Add(TaskContentDto newTaskContent)
        {
            try
            {
                var taskContent = new TaskContentModel(
                newTaskContent.CreatorId,
                newTaskContent.CategoryId,
                newTaskContent.MaterialsId,
                newTaskContent.Title ?? "",
                newTaskContent.Description ?? "");
                await _context.AddAsync(taskContent);
                await _context.SaveChangesAsync();
                newTaskContent.Id = taskContent.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Add taskContent: " + ex);
            }
            return newTaskContent;
        }

        public async Task<bool> Edit(int taskContentId, TaskContentDto newTaskContent)
        {
            var taskContent = await _context.TaskContents.FindAsync(taskContentId);
            if (taskContent == null)
            {
                return false;
            }
            try
            {
                taskContent.CreatorId = newTaskContent.CreatorId;
                taskContent.CategoryId = newTaskContent.CategoryId;
                taskContent.MaterialsId = newTaskContent.MaterialsId;
                taskContent.Title = newTaskContent.Title ?? "";
                taskContent.Description = newTaskContent.Description ?? "";
                _context.TaskContents.Update(taskContent);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Edit taskContent:" + ex);
            }
            return true;
        }

        public async Task<bool> Delete(int taskContentId)
        {
            var taskContent = await _context.TaskContents.FindAsync(taskContentId);
            if (taskContent == null)
            {
                return false;
            }
            try
            {
                taskContent.State = StateEnum.Deleted;
                _context.TaskContents.Update(taskContent);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Delete taskContent:" + ex);
            }
            return true;
        }

        public async Task<List<TaskContentDto>> GetAll()
        {
            var taskContents = await _context.TaskContents
                .Where(tc => tc.State != StateEnum.Deleted)
                .Select(tc => new TaskContentDto
                {
                    Id = tc.Id,
                    CreatorId = tc.CreatorId,
                    CategoryId = tc.CategoryId,
                    MaterialsId = tc.MaterialsId,
                    Title = tc.Title,
                    Description = tc.Description
                }).ToListAsync();

            return taskContents;
        }
        public async Task<TaskContentDto> GetById(int taskContentId)
        {
            var taskContent = await _context.TaskContents
                .Where(tc => tc.Id == taskContentId && tc.State != StateEnum.Deleted)
                .Select(tc => new TaskContentDto
                {
                    Id = tc.Id,
                    CreatorId = tc.CreatorId,
                    CategoryId = tc.CategoryId,
                    MaterialsId = tc.MaterialsId,
                    Title = tc.Title,
                    Description = tc.Description

                }).FirstOrDefaultAsync();

            if (taskContent == null)
            {
                throw new Exception("TaskContent not found");
            }

            return taskContent;
        }

        public async Task<List<TaskContentDto>> GetByTitle(string title)
        {
            var taskContents = await _context.TaskContents
                .Where(tc => tc.Title.Contains(title) && tc.State != StateEnum.Deleted)
                .Select(tc => new TaskContentDto
                {
                    Id = tc.Id,
                    CreatorId = tc.CreatorId,
                    CategoryId = tc.CategoryId,
                    MaterialsId = tc.MaterialsId,
                    Title = tc.Title,
                    Description = tc.Description
                }).ToListAsync();

            if (taskContents == null || taskContents.Count == 0)
            {
                throw new Exception("No TaskContent found");
            }
            return taskContents;
        }

        public async Task<List<TaskContentDto>> GetByCreatorId(Guid creatorId)
        {
            var taskContents = await _context.TaskContents
                .Where(tc => tc.CreatorId == creatorId && tc.State != StateEnum.Deleted)
                .Select(tc => new TaskContentDto
                {
                    Id = tc.Id,
                    CreatorId = tc.CreatorId,
                    CategoryId = tc.CategoryId,
                    MaterialsId = tc.MaterialsId,
                    Title = tc.Title,
                    Description = tc.Description
                }).ToListAsync();

            return taskContents;
        }

        public async Task<List<TaskContentDto>> GetByCategoryId(int categoryId)
        {
            var taskContents = await _context.TaskContents
                .Where(tc => tc.CategoryId == categoryId && tc.State != StateEnum.Deleted)
                .Select(tc => new TaskContentDto
                {
                    Id = tc.Id,
                    CreatorId = tc.CreatorId,
                    CategoryId = tc.CategoryId,
                    MaterialsId = tc.MaterialsId,
                    Title = tc.Title,
                    Description = tc.Description
                }).ToListAsync();

            return taskContents;
        }
    }
}
