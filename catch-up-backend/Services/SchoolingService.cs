using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using catch_up_backend.Response;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace catch_up_backend.Services
{
    public class SchoolingService : ISchoolingService
    {
        private readonly CatchUpDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly ISchoolingPartService _schoolingPartService;
        private readonly IFileService _fileService;

        public SchoolingService(CatchUpDbContext context, ICategoryService categoryService, ISchoolingPartService schoolingPartService, IFileService fileService)
        {
            _context = context;
            _categoryService = categoryService;
            _schoolingPartService = schoolingPartService;
            _fileService = fileService;
        }

/*        public async Task<SchoolingPartDto> CreateSchoolingPart(SchoolingPartDto schoolingPart, int schoolingID)
        {
            if (schoolingPart == null || schoolingID <= 0)
                return null;

            var schoolingPartModel = new SchoolingPartModel(schoolingID, schoolingPart.Title, schoolingPart.Content, "",null);
            _context.SchoolingParts.Add(schoolingPartModel);
            await _context.SaveChangesAsync();
            schoolingPart.Id = schoolingPartModel.Id;

            if (schoolingPart.Materials != null && schoolingPart.Materials.Any())
            {
                foreach (var materialDto in schoolingPart.Materials)
                {
                    var materialSchoolingPart = new MaterialsSchoolingPartModel(materialDto.Id, schoolingPartModel.Id);
                    _context.MaterialsSchoolingParts.Add(materialSchoolingPart);
                }
                await _context.SaveChangesAsync();
            }
            return schoolingPart;
        }*/


        //

        public async Task<bool> EditSchooling(SchoolingDto schoolingDto)
        {
            var existingSchooling = await _context.Schoolings.FindAsync(schoolingDto.Id);
            if(existingSchooling == null)
                return false;

            if(!await _categoryService.IsActive(existingSchooling.CategoryId))
                return false;
            existingSchooling.Title = schoolingDto.Title;
            existingSchooling.ShortDescription = schoolingDto.ShortDescription;
            existingSchooling.Priority = schoolingDto.Priority;
            existingSchooling.CategoryId = schoolingDto.CategoryId;
            existingSchooling.Content = schoolingDto.Content;
            existingSchooling.IconFileId = schoolingDto.IconFile?.Id;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SchoolingDto> GetById(int schoolingId)
        {
            var schoolingModel = await _context.Schoolings.FindAsync(schoolingId);
            if (schoolingModel == null)
                return null;

            var schooling = new SchoolingDto(schoolingModel);
            
            schooling.SchoolingPartProgressBar = await _schoolingPartService.GetSchoolingPartStatus(schoolingId);

            if (schoolingModel.IconFileId != null)
                schooling.IconFile = await _fileService.GetById((int)schoolingModel.IconFileId);

            return schooling;
        }
        public async Task<SchoolingDto> GetById(int schoolingId, Guid userId)
        {
            var schoolingModel = await _context.Schoolings.FindAsync(schoolingId);
            if (schoolingModel == null)
                return null;
            var schooling = new SchoolingDto(schoolingModel);
            schooling.SchoolingPartProgressBar = await _schoolingPartService.GetUserSchoolingPartStatus(schoolingId, userId);

            if (schoolingModel.IconFileId != null)
                schooling.IconFile = await _fileService.GetById((int)schoolingModel.IconFileId);
            return schooling;
        }

        public async Task<PagedResponse<SchoolingDto>> GetOwnedSchoolingsAsync(SchoolingQueryParameters parameters, Guid userId)
        {
            var query = _context.Schoolings.AsQueryable();

            query = query.Where(s => s.CreatorId == userId);
            query = query.Where(s => s.State == StateEnum.Active);
            query = ApplyFilters(query, parameters);
            query = ApplySorting(query, parameters);

            // Liczenie całkowitej liczby rekordów
            var totalCount = await query.CountAsync();

            // Paginacja
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            // Mapowanie do DTO
            var schoolingDtos = items.Select(s => new SchoolingDto(s)).ToList();

            return new PagedResponse<SchoolingDto>(schoolingDtos, parameters.PageNumber, parameters.PageSize, totalCount);
        }

        public async Task<PagedResponse<SchoolingDto>> GetSchoolingsAsync(SchoolingQueryParameters parameters)
        {
            var query = _context.Schoolings.AsQueryable();

            query = query.Where(s => s.State == StateEnum.Active);
            query = ApplyFilters(query, parameters);
            query = ApplySorting(query, parameters);

            // Liczenie całkowitej liczby rekordów
            var totalCount = await query.CountAsync();

            // Paginacja
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            // Mapowanie do DTO
            var schoolingDtos = items.Select(s => new SchoolingDto(s)).ToList();

            return new PagedResponse<SchoolingDto>(schoolingDtos, parameters.PageNumber, parameters.PageSize, totalCount);
        }

        public async Task<PagedResponse<SchoolingDto>> GetSubscribedSchoolingsAsync(SchoolingQueryParameters parameters, Guid userId)
        {
            var query = from su in _context.SchoolingsUsers.AsQueryable()
                        join s in _context.Schoolings.AsQueryable() on su.SchoolingId equals s.Id
                        where su.State == StateEnum.Active && su.NewbieId == userId
                        select s;

            query = ApplyFilters(query, parameters);
            query = ApplySorting(query, parameters);

            // Liczenie całkowitej liczby rekordów
            var totalCount = await query.CountAsync();

            // Paginacja
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            // Mapowanie do DTO
            var schoolingDtos = items.Select(s => new SchoolingDto(s)).ToList();

            return new PagedResponse<SchoolingDto>(schoolingDtos, parameters.PageNumber, parameters.PageSize, totalCount);
        }
        private IQueryable<SchoolingModel> ApplyFilters(IQueryable<SchoolingModel> query, SchoolingQueryParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.TitleFilter))
            {
                query = query.Where(p => p.Title.Contains(parameters.TitleFilter));
            }

            if (!(parameters.CategoryFilter is null))
            {
                query = query.Where(p => p.CategoryId == parameters.CategoryFilter);
            }
            return query;
        }

        private IQueryable<SchoolingModel> ApplySorting(IQueryable<SchoolingModel> query, SchoolingQueryParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                var isAscending = parameters.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);
                switch (parameters.SortBy.ToLower())
                {
                    case "title":
                        query = isAscending ? query.OrderBy(p => p.Title) : query.OrderByDescending(p => p.Title);
                        break;
                    case "priority":
                        query = isAscending ? query.OrderBy(p => p.Priority) : query.OrderByDescending(p => p.Priority);
                        break;
                    default:
                        query = query.OrderBy(p => p.Id);
                        break;
                }
            }
            else
                query = query.OrderBy(p => p.Id);

            return query;
        }
    }
}
