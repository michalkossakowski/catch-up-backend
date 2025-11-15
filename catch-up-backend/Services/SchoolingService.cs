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

        public async Task<SchoolingDto> CreateSchoolingAsync(SchoolingDto schooling)
        {
            SchoolingModel schoolingModel;
            try
            {
                schoolingModel = new SchoolingModel(
                    schooling.CreatorId,
                    schooling.CategoryId,
                    schooling.Title,
                    schooling.ShortDescription,
                    schooling.Content,
                    schooling.Priority
                );
                _context.Schoolings.Add(schoolingModel);
                _context.SaveChanges();
                foreach (var part in schooling.schoolingParts)
                {
                    var schoolingPartModel = new SchoolingPartModel(
                        schoolingModel.Id,
                        part.Title,
                        part.ShortDescription,
                        part.Content
                    );
                    _context.SchoolingParts.Add(schoolingPartModel);
                }
                _context.SaveChanges();
            }catch (Exception)
            {
                return null;
            }
            return await GetById(schoolingModel.Id);
        }


        public async Task<bool> EditSchoolingAsync(SchoolingDto schoolingDto)
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

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SchoolingDto> GetById(int schoolingId)
        {
            var schoolingModel = await _context.Schoolings.FindAsync(schoolingId);
            if (schoolingModel == null)
                return null;

            var schooling = new SchoolingDto(schoolingModel);
            
            schooling.schoolingParts = await _schoolingPartService.GetSchoolingParts(schoolingId);


            return schooling;
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

        public async Task<bool> DeleteSchoolingAsync(int schoolingId)
        {
            var schooling = await _context.Schoolings.FindAsync(schoolingId);
            if (schooling == null)
                return false;
            try
            {
                schooling.State = StateEnum.Deleted;
                var parts = _context.SchoolingParts.Where(p => p.SchoolingId == schooling.Id).ToList();
                foreach(var part in parts)
                {
                    part.State = StateEnum.Deleted;
                }
                await _context.SaveChangesAsync();
            }catch (Exception)
            {
                return false;
            }
            return true;

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
