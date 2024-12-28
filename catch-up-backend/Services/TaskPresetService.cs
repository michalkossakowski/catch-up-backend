using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Services.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;

namespace catch_up_backend.Services
{
    public class TaskPresetService : ITaskPresetService
    {
        private readonly CatchUpDbContext _context;

        public TaskPresetService(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task<TaskPresetDto> Add(TaskPresetDto newTaskPreset)
        {
            try
            {
                var taskPreset = new TaskPresetModel(
                    newTaskPreset.PresetId,
                    newTaskPreset.TaskContentId);
                await _context.AddAsync(taskPreset);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: AddAsync taskPreset: " + ex);
            }
            return newTaskPreset;
        }

        public async Task<bool> Edit(int taskPresetId, TaskPresetDto newTaskPreset)
        {
            var taskPreset = await _context.TaskPresets
                .FirstOrDefaultAsync(tp => tp.PresetId == taskPresetId);
            if (taskPreset == null)
            {
                return false;
            }
            try
            {
                taskPreset.PresetId = newTaskPreset.PresetId;
                taskPreset.TaskContentId = newTaskPreset.TaskContentId;
                taskPreset.State = newTaskPreset.State;
                _context.TaskPresets.Update(taskPreset);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: EditAsync taskPreset:" + ex);
            }
            return true;
        }

        public async Task<bool> Delete(int taskPresetId)
        {
            var taskPreset = await _context.TaskPresets
                .FirstOrDefaultAsync(tp => tp.PresetId == taskPresetId);
            if (taskPreset == null)
            {
                return false;
            }
            try
            {
                taskPreset.State = StateEnum.Deleted;
                _context.TaskPresets.Update(taskPreset);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: DeleteAsync taskPreset:" + ex);
            }
            return true;
        }

        public async Task<bool> RemoveTaskFromPreset(int taskPresetId, int taskContentId)
        {
            var taskPreset = await _context.TaskPresets
                .FirstOrDefaultAsync(tp => tp.PresetId == taskPresetId && tp.TaskContentId == taskContentId);
            if (taskPreset == null)
            {
                return false;
            }
            try
            {
                taskPreset.State = StateEnum.Deleted;
                _context.TaskPresets.Update(taskPreset);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Remove task from preset:" + ex);
            }
            return true;
        }

        public async Task<bool> RemoveTaskFromAllPresets(int taskContentId)
        {
            var taskPresets = await _context.TaskPresets
                .Where(tp => tp.TaskContentId == taskContentId)
                .ToListAsync();

            try
            {
                foreach (var taskPreset in taskPresets)
                {
                    taskPreset.State = StateEnum.Deleted;
                }
                _context.TaskPresets.UpdateRange(taskPresets);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Remove task from all presets:" + ex);
            }
        }

        public async Task<List<TaskPresetDto>> GetAll()
        {
            var taskPresets = await _context.TaskPresets
                .Where(tp => tp.State != StateEnum.Deleted)
                .Select(tp => new TaskPresetDto
                {
                    PresetId = tp.PresetId,
                    TaskContentId = tp.TaskContentId,
                    State = tp.State
                }).ToListAsync();

            return taskPresets;
        }

        public async Task<List<TaskPresetDto>> GetById(int taskPresetId)
        {
            var taskPresets = await _context.TaskPresets
                .Where(tp => tp.PresetId == taskPresetId && tp.State != StateEnum.Deleted)
                .Select(tp => new TaskPresetDto
                {
                    PresetId = tp.PresetId,
                    TaskContentId = tp.TaskContentId,
                    State = tp.State
                }).ToListAsync();

            return taskPresets;
        }

        public async Task<List<TaskPresetDto>> GetByTaskContent(int taskContentId)
        {
            var taskPresets = await _context.TaskPresets
                .Where(tp => tp.TaskContentId == taskContentId && tp.State != StateEnum.Deleted)
                .Select(tp => new TaskPresetDto
                {
                    PresetId = tp.PresetId,
                    TaskContentId = tp.TaskContentId,
                    State = tp.State
                }).ToListAsync();

            return taskPresets;
        }

        public async Task<List<TaskPresetDto>> GetByPresetId(int presetId)
        {
            var taskPresets = await _context.TaskPresets
                .Where(tp => tp.PresetId == presetId && tp.State != StateEnum.Deleted)
                .Select(tp => new TaskPresetDto
                {
                    PresetId = tp.PresetId,
                    TaskContentId = tp.TaskContentId,
                    State = tp.State
                }).ToListAsync();

            return taskPresets;
        }

        public async Task<bool> DeleteByPresetId(int presetId)
        {
            var taskPresets = await _context.TaskPresets
                .Where(tp => tp.PresetId == presetId)
                .ToListAsync();

            if (!taskPresets.Any())
            {
                return false;
            }

            try
            {
                foreach (var taskPreset in taskPresets)
                {
                    taskPreset.State = StateEnum.Deleted;
                }
                _context.TaskPresets.UpdateRange(taskPresets);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: DeleteAsync taskPreset:" + ex);
            }
            return true;
        }
    }
} 