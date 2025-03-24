using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Database;

public class EventService : IEventService
{
    private readonly CatchUpDbContext _context;

    public EventService(CatchUpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetUserEvents(Guid userId)
    {
        return await _context.Events
            .Where(e => e.OwnerId == userId || e.ReceiverIds.Contains(userId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetFullEvents()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task AddEventByPosition(Guid ownerId, string title, string position, DateTime startDate, DateTime endDate)
    {
        var receivers = await _context.Users
            .Where(u => u.Position == position)
            .Select(u => u.Id)
            .ToListAsync();

        var eventEntry = new Event
        {
            Title = title,
            StartDate = startDate,
            EndDate = endDate,
            OwnerId = ownerId,
            ReceiverIds = receivers
        };

        _context.Events.Add(eventEntry);
        await _context.SaveChangesAsync();
    }

    public async Task AddEventByType(Guid ownerId, string title, string type, DateTime startDate, DateTime endDate)
    {
        var receivers = await _context.Users
            .Where(u => u.Type == type)
            .Select(u => u.Id)
            .ToListAsync();

        var eventEntry = new Event
        {
            Title = title,
            StartDate = startDate,
            EndDate = endDate,
            OwnerId = ownerId,
            ReceiverIds = receivers
        };

        _context.Events.Add(eventEntry);
        await _context.SaveChangesAsync();
    }

    public async Task AddEventForAllGroups(Guid ownerId, string title, DateTime startDate, DateTime endDate)
    {
        var receivers = await _context.Users
            .Select(u => u.Id)
            .ToListAsync();

        var eventEntry = new Event
        {
            Title = title,
            StartDate = startDate,
            EndDate = endDate,
            OwnerId = ownerId,
            ReceiverIds = receivers
        };

        _context.Events.Add(eventEntry);
        await _context.SaveChangesAsync();
    }
}