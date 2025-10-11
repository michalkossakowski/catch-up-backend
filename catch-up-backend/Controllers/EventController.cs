using catch_up_backend.Dtos;
using catch_up_backend.Helpers;
using catch_up_backend.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> Get()
    {
        var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
        var events = await _eventService.GetUserEvents(userId);

        return Ok(events);
    }

    [HttpDelete]
    [Route("Delete/{eventId:int}")]
    public async Task<ActionResult<IEnumerable<EventModel>>> Delete(int eventId)
    {
        var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
        return await _eventService.DeleteAsync(userId, eventId)
            ? Ok(new { message = $"Event '{eventId}' deleted" })
            : NotFound(new { message = $"Event with id: '{eventId}' not found or user don't have permission" });
    }

    [HttpPost]
    public async Task<ActionResult> Post(EventDto eventDto)
    {
        var result =  await _eventService.AddAsync(eventDto);

        return result != null
            ? Ok(new { message = "Event added", eventDto = result })
            : StatusCode(500, new { message = "Event adding error" });
    }
}