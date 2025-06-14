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
            if (updatedSettings == null || !updatedSettings.Any())
            {
                return BadRequest(new { message = "Settings data is missing or invalid" });
            }
            bool result = await _companySettingsService.UpdateSettings(updatedSettings);
            if (result)
            {
                return Ok(new { message = "Settings updated successfully", updatedSettings });
            }
            return NotFound(new { message = "Failed to update settings" });
        }
        [HttpGet]
        [Route("GetWithoutMessage")]
        public async Task<IActionResult> GetCompanySettingsWithoutMessage()
        {
            var settings = await _companySettingsService.GetCompanySettings();
            if (settings == null)
            {
                return NotFound(new { messege = "Settings not found" });
            }
            return Ok(settings);
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
        [HttpPatch]
        [Route("TurnOnLocalization")]
        public async Task<IActionResult> TurnOnLocalization()
        {
            bool result = await _companySettingsService.TurnOnLocalization();
            if (result)
            {
                return Ok(new { message = "Localization turned on" });
            }
            else
            {
                return NotFound(new { message = "Failed to turn on/off localization" });
            }
        }
        [HttpPatch]
        [Route("TurnOffLocalization")]
        public async Task<IActionResult> TurnOffLocalization()
        {
            bool result = await _companySettingsService.TurnOffLocalization();
            if (!result)
            {
                return Ok(new { message = "Localization turned off" });
            }
            else
            {
                return NotFound(new { message = "Failed to turn on/off localization" });
            }
        }
        [HttpPatch]
        [Route("SetTaskTimeLoggingSetting/{enable:bool}")]
        public async Task<IActionResult> SetTaskTimeLogging(bool enable)
        {
            bool? result = await _companySettingsService.SetTaskTimeLogging(enable);
            if (result.HasValue)
            {
                return Ok(new { message = $"Task time logging has been set to {enable}" });
            }
            else
            {
                return NotFound(new { message = "Failed to set task time logging" });
            }
        }
        [HttpGet]
        [Route("GetTaskTimeLoggingSetting")]
        public async Task<IActionResult> GetTaskTimeLoggingSetting()
        {
            bool result = await _companySettingsService.GetTaskTimeLoggingSetting();
            return Ok(new { message = "Task time logging setting retrieved successfully", enable = result });
        }
    }
}
