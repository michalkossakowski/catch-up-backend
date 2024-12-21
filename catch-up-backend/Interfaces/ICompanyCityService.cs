using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ICompanyCityService
    {
        Task<IEnumerable<CompanyCityDto>> GetAllCitiesAsync();
        Task<CompanyCityDto> GetCityByNameAsync(string cityName);
        Task<bool> AddCityAsync(CompanyCityDto cityDto);
        Task<bool> DeleteCityAsync(string cityName);
    }
}
