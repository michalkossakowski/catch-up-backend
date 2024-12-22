using catch_up_backend.Interfaces;
using catch_up_backend.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyCityController : ControllerBase
    {
        private readonly ICompanyCityService _companyCityService;

        public CompanyCityController(ICompanyCityService companyCityService)
        {
            _companyCityService = companyCityService;
        }

        [HttpGet]
        [Route("GetAllCities")]
        public async Task<ActionResult<IEnumerable<CompanyCityDto>>> GetAllCities()
        {
            var cities = await _companyCityService.GetAllCitiesAsync();
            if (cities == null)
            {
                return NotFound("No cities found.");
            }
            return Ok(cities);
        }

        [HttpGet]
        [Route("GetCityByName/{cityName}")]
        public async Task<ActionResult<CompanyCityDto>> GetCityByName(string cityName)
        {
            var city = await _companyCityService.GetCityByNameAsync(cityName);
            if (city == null)
            {
                return NotFound($"City with name {cityName} not found.");
            }
            return Ok(city);
        }

        [HttpPost]
        [Route("AddCity")]
        public async Task<ActionResult> AddCity([FromBody] CompanyCityDto cityDto)
        {
            if (cityDto == null)
            {
                return BadRequest("City data is required.");
            }

            var result = await _companyCityService.AddCityAsync(cityDto);
            if (result)
            {
                return CreatedAtAction(nameof(GetCityByName), new { cityName = cityDto.CityName }, cityDto);
            }
            return StatusCode(500, "Error occurred while adding the city.");
        }


        [HttpDelete]
        [Route("DeleteCity/{cityName}")]
        public async Task<ActionResult> DeleteCity(string cityName)
        {
            var result = await _companyCityService.DeleteCityAsync(cityName);
            if (result)
            {
                return NoContent();
            }
            return NotFound($"City with name {cityName} not found.");
        }
    }
}
