using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class CompanyCityService : ICompanyCityService
    {
        private readonly CatchUpDbContext _context;

        public CompanyCityService(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyCityDto>> GetAllCitiesAsync()
        {
            return await _context.CompanyCities
            .Select(city => new CompanyCityDto
            {
              CityName = city.CityName,
              Latitude = city.Latitude,
              Longitude = city.Longitude,
              RadiusKm = city.RadiusKm
            })
            .ToListAsync();
        }

        public async Task<CompanyCityDto> GetCityByNameAsync(string cityName)
        {
            var city = await _context.CompanyCities
            .FirstOrDefaultAsync(c => c.CityName == cityName);

            if (city == null)
            {
                return null;
            }

            return new CompanyCityDto
            {
                CityName = city.CityName,
                Latitude = city.Latitude,
                Longitude = city.Longitude,
                RadiusKm = city.RadiusKm
            };
        }

        public async Task<bool> AddCityAsync(CompanyCityDto cityDto)
        {
            CompanyCity city = new CompanyCity
            {
                CityName = cityDto.CityName,
                Latitude = cityDto.Latitude,
                Longitude = cityDto.Longitude,
                RadiusKm = cityDto.RadiusKm
            };

            _context.CompanyCities.Add(city);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCityAsync(string cityName)
        {
            var city = await _context.CompanyCities
            .FirstOrDefaultAsync(c => c.CityName == cityName);

            if (city == null)
            {
                return false;
            }

            _context.CompanyCities.Remove(city);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
