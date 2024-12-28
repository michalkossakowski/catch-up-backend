using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Services.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;

namespace catch_up_backend.Services
{
    public class PresetService : IPresetService
    {
        private readonly CatchUpDbContext _context;

        public PresetService(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task<PresetDto> Add(PresetDto newPreset)
        {
            try
            {
                if (string.IsNullOrEmpty(newPreset.Name))
                    throw new ArgumentException("Preset name cannot be empty");

                if (string.IsNullOrEmpty(newPreset.CreatorId))
                    throw new ArgumentException("Creator ID cannot be empty");

                var preset = new PresetModel(
                    Guid.Parse(newPreset.CreatorId),
                    newPreset.Name);

                await _context.AddAsync(preset);
                await _context.SaveChangesAsync();
                
                newPreset.Id = preset.Id;
                return newPreset;
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid Creator ID format", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding preset: {ex.Message}", ex);
            }
        }

        public async Task<PresetDto> Edit(PresetDto newPreset)
        {
            var preset = await _context.Presets.FindAsync(newPreset.Id);
            if (preset == null)
            {
                throw new Exception("Preset not found");
            }
            try
            {
                preset.CreatorId = Guid.Parse(newPreset.CreatorId);
                preset.Name = newPreset.Name;
                _context.Presets.Update(preset);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: EditAsync preset:" + ex);
            }
            return newPreset;
        }

        public async Task<bool> DeletePreset(int presetId)
        {
            var preset = await _context.Presets.FindAsync(presetId);
            if (preset == null)
            {
                return false;
            }
            try
            {
                var taskPresets = await _context.TaskPresets
                    .Where(tp => tp.PresetId == presetId)
                    .ToListAsync();
                    
                foreach (var taskPreset in taskPresets)
                {
                    taskPreset.State = StateEnum.Deleted;
                }
                _context.TaskPresets.UpdateRange(taskPresets);

                preset.State = StateEnum.Deleted;
                _context.Presets.Update(preset);
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: DeleteAsync preset:" + ex);
            }
            return true;
        }

        public async Task<List<PresetDto>> GetAll()
        {
            var presets = await _context.Presets
                .Where(p => p.State != StateEnum.Deleted)
                .Select(p => new PresetDto
                {
                    Id = p.Id,
                    CreatorId = p.CreatorId.ToString(),
                    Name = p.Name,
                    State = p.State
                }).ToListAsync();

            return presets;
        }

        public async Task<PresetDto> GetById(int presetId)
        {
            var preset = await _context.Presets
                .Where(p => p.Id == presetId && p.State != StateEnum.Deleted)
                .Select(p => new PresetDto
                {
                    Id = p.Id,
                    CreatorId = p.CreatorId.ToString(),
                    Name = p.Name,
                    State = p.State
                }).FirstOrDefaultAsync();

            if (preset == null)
            {
                throw new Exception("Preset not found");
            }

            return preset;
        }

        public async Task<List<PresetDto>> GetByCreatorId(Guid creatorId)
        {
            var presets = await _context.Presets
                .Where(p => p.CreatorId == creatorId && p.State != StateEnum.Deleted)
                .Select(p => new PresetDto
                {
                    Id = p.Id,
                    CreatorId = p.CreatorId.ToString(),
                    Name = p.Name,
                    State = p.State
                }).ToListAsync();

            return presets;
        }

        public async Task<List<PresetDto>> GetByName(string name)
        {
            var presets = await _context.Presets
                .Where(p => p.Name.Contains(name) && p.State != StateEnum.Deleted)
                .Select(p => new PresetDto
                {
                    Id = p.Id,
                    CreatorId = p.CreatorId.ToString(),
                    Name = p.Name,
                    State = p.State
                }).ToListAsync();

            return presets;
        }

        public async Task<List<PresetDto>> GetByTaskContent(int taskContentId)
        {
            var presets = await _context.TaskPresets
                .Where(tp => tp.TaskContentId == taskContentId && tp.State != StateEnum.Deleted)
                .Join(_context.Presets,
                    tp => tp.PresetId,
                    p => p.Id,
                    (tp, p) => new PresetDto
                    {
                        Id = p.Id,
                        CreatorId = p.CreatorId.ToString(),
                        Name = p.Name,
                        State = p.State
                    })
                .ToListAsync();

            return presets;
        }

        public async Task<List<PresetDto>> SearchPresets(string searchingString)
        {
            var presets = await _context.Presets
                .Where(p => p.Name.Contains(searchingString) && p.State != StateEnum.Deleted)
                .Select(p => new PresetDto
                {
                    Id = p.Id,
                    CreatorId = p.CreatorId.ToString(),
                    Name = p.Name,
                    State = p.State
                }).ToListAsync();

            return presets;
        }
    }
} 