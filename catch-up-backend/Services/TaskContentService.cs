using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;
using catch_up_backend.Response;

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

        public async Task<TaskContentDto> Edit(int taskContentId, TaskContentDto newTaskContent)
        {
            var taskContent = await _context.TaskContents.FindAsync(taskContentId);
            if (taskContent == null)
                return null;
            try
            {
                taskContent.CreatorId = newTaskContent.CreatorId;
                taskContent.CategoryId = newTaskContent.CategoryId;
                taskContent.MaterialsId = newTaskContent.MaterialsId;
                taskContent.Title = newTaskContent.Title ?? "";
                taskContent.Description = newTaskContent.Description ?? "";
                _context.TaskContents.Update(taskContent);
                await _context.SaveChangesAsync();
                newTaskContent.Id = taskContent.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Edit taskContent:" + ex);
            }
            return newTaskContent;
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

        public async Task<(List<TaskContentDto> taskContents, int totalCount)> GetAll(int page, int pageSize)
        {
            var query = _context.TaskContents.Where(tc => tc.State != StateEnum.Deleted);
            var totalCount = await query.CountAsync();

            var taskContents = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(tc => new TaskContentDto
                {
                    Id = tc.Id,
                    CreatorId = tc.CreatorId,
                    CategoryId = tc.CategoryId,
                    MaterialsId = tc.MaterialsId,
                    Title = tc.Title,
                    Description = tc.Description
                })
                .ToListAsync();

            return (taskContents, totalCount);
        }

        public async Task<PagedResponse<TaskContentDto>> GetTaskContentsAsync(TaskContentQueryParameters parameters)
        {
            var query = _context.TaskContents.Where(tc => tc.State != StateEnum.Deleted);

            if (!string.IsNullOrEmpty(parameters.TitleFilter))
            {
                query = query.Where(tc => tc.Title.Contains(parameters.TitleFilter));
            }

            if (parameters.CategoryFilter.HasValue)
            {
                query = query.Where(tc => tc.CategoryId == parameters.CategoryFilter.Value);
            }

            if (parameters.CreatorFilter.HasValue)
            {
                query = query.Where(tc => tc.CreatorId == parameters.CreatorFilter.Value);
            }

            query = parameters.SortBy?.ToLower() switch
            {
                "title" => parameters.SortOrder.ToLower() == "desc" 
                    ? query.OrderByDescending(tc => tc.Title)
                    : query.OrderBy(tc => tc.Title),
                "category" => parameters.SortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(tc => tc.CategoryId)
                    : query.OrderBy(tc => tc.CategoryId),
                _ => query.OrderBy(tc => tc.Title)
            };

            var totalCount = await query.CountAsync();

            var taskContents = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(tc => new TaskContentDto
                {
                    Id = tc.Id,
                    CreatorId = tc.CreatorId,
                    CategoryId = tc.CategoryId,
                    MaterialsId = tc.MaterialsId,
                    Title = tc.Title,
                    Description = tc.Description
                })
                .ToListAsync();

            return new PagedResponse<TaskContentDto>(taskContents, parameters.PageNumber, parameters.PageSize, totalCount);
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
