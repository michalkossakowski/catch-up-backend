using Microsoft.AspNetCore.Mvc;
using catch_up_backend.Dtos;
using catch_up_backend.Services.Interfaces;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresetController : ControllerBase
    {
        private readonly IPresetService _presetService;

        public PresetController(IPresetService presetService)
        {
            _presetService = presetService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] PresetDto newPreset)
        {
            try
            {
                if (newPreset == null)
                    return BadRequest("Preset data is null");

                var result = await _presetService.Add(newPreset);
                return Ok(new { message = "Preset added", preset = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPut]
        [Route("Edit/{presetId:int}")]
        public async Task<IActionResult> Edit(int presetId, [FromBody] PresetDto newPreset)
        {
            var result = await _presetService.Edit(presetId, newPreset);
            return result != null
                ? Ok(new { message = "Preset edited", preset = result })
                : StatusCode(500, new { message = "Error: Preset edit" });
        }

        [HttpDelete]
        [Route("Delete/{presetId:int}")]
        public async Task<IActionResult> Delete(int presetId)
        {
            return await _presetService.DeletePreset(presetId)
                ? Ok(new { message = "Preset deleted", preset = presetId })
                : NotFound(new { message = "Error: Preset delete", preset = presetId });
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var presets = await _presetService.GetAll();
                if (!presets.Any())
                    return NotFound(new { message = "No presets found" });
                return Ok(presets);
            }
            catch (Exception)
            {
                return NotFound(new { message = "Error retrieving presets" });
            }
        }

        [HttpGet]
        [Route("GetById/{presetId:int}")]
        public async Task<IActionResult> GetById(int presetId)
        {
            try
            {
                var preset = await _presetService.GetById(presetId);
                if (preset == null)
                    return NotFound(new { message = $"Preset with id: {presetId} not found" });
                return Ok(preset);
            }
            catch (Exception)
            {
                return NotFound(new { message = $"Preset with id: {presetId} not found" });
            }
        }

        [HttpGet]
        [Route("GetByCreatorId/{creatorId:Guid}")]
        public async Task<IActionResult> GetByCreatorId(Guid creatorId)
        {
            try
            {
                var presets = await _presetService.GetByCreatorId(creatorId);
                if (!presets.Any())
                    return NotFound(new { message = $"No presets found for creator with id: {creatorId}" });
                return Ok(presets);
            }
            catch (Exception)
            {
                return NotFound(new { message = $"Error retrieving presets for creator with id: {creatorId}" });
            }
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var presets = await _presetService.GetByName(name);
                if (!presets.Any())
                    return NotFound(new { message = $"No presets found with name containing: {name}" });
                return Ok(presets);
            }
            catch (Exception)
            {
                return NotFound(new { message = $"Error retrieving presets with name containing: {name}" });
            }
        }

        [HttpGet]
        [Route("GetByTaskContent/{taskContentId:int}")]
        public async Task<IActionResult> GetByTaskContent(int taskContentId)
        {
            try
            {
                var presets = await _presetService.GetByTaskContent(taskContentId);
                if (!presets.Any())
                    return NotFound(new { message = $"No presets found for task content with id: {taskContentId}" });
                return Ok(presets);
            }
            catch (Exception)
            {
                return NotFound(new { message = $"Error retrieving presets for task content with id: {taskContentId}" });
            }
        }

        [HttpGet]
        [Route("SearchPresets/{searchingString}")]
        public async Task<IActionResult> SearchPresets(string searchingString)
        {
            try
            {
                var presets = await _presetService.SearchPresets(searchingString);
                if (!presets.Any())
                    return NotFound(new { message = $"No presets found matching: {searchingString}" });
                return Ok(presets);
            }
            catch (Exception)
            {
                return NotFound(new { message = $"Error searching presets with query: {searchingString}" });
            }
        }
    }
} 