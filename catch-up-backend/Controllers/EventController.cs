using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [HttpGet("GetUserEvents/{id}")]
    public async Task<ActionResult<IEnumerable<EventModel>>> GetUserEvents(Guid id)
    {
        var events = await _eventService.GetUserEvents(id);
        return Ok(events);
    }

    [HttpGet("GetFullEvents")]
    public async Task<ActionResult<IEnumerable<EventModel>>> GetFullEvents()
    {
        var events = await _eventService.GetFullEvents();
        return Ok(events);
    }

    [HttpPost("AddEventByPosition")]
    public async Task<ActionResult> AddEventByPosition(Guid ownerId, string title, string description, string position, DateTime startDate, DateTime endDate)
    {
        await _eventService.AddEventByPosition(ownerId, title, description, position, startDate, endDate);
        return Ok();
    }

    [HttpPost("AddEventByType")]
    public async Task<ActionResult> AddEventByType(Guid ownerId, string title, string description, string type, DateTime startDate, DateTime endDate)
    {
        await _eventService.AddEventByType(ownerId, title, description, type, startDate, endDate);
        return Ok();
    }

    [HttpPost("AddEventForAllGroups")]
    public async Task<ActionResult> AddEventForAllGroups(Guid ownerId, string title, string description, DateTime startDate, DateTime endDate)
    {
        await _eventService.AddEventForAllGroups(ownerId, title, description, startDate, endDate);
        return Ok();
    }
}