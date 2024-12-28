using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanySettingsController : ControllerBase
    {
        private readonly ICompanySettingsService _companySettingsService;

        public CompanySettingsController(ICompanySettingsService companySettingsService)
        {
            _companySettingsService = companySettingsService;
        }

        [HttpPost]
        [Route("UpdateAll")]
        public async Task<IActionResult> UpdateSettings([FromBody] Dictionary<string, bool> updatedSettings)
        {
            if(updatedSettings == null || !updatedSettings.Any())
            {
                return BadRequest(new { message = "Settings data is missing or invalid" });
            }
            bool result = await _companySettingsService.UpdateSettings(updatedSettings);
            if (result)
            {
                return Ok(new { message = "Settings updated successfully", updatedSettings });
            }
            return NotFound(new {message= "Failed to update settings"});
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetCompanySettings()
        {
            var settings = await _companySettingsService.GetCompanySettings();
            if (settings == null)
            {
                return NotFound(new { messege = "Settings not found" });
            }
            return Ok(new { message = "Settings get successfully", settings });
        }
    }
}
