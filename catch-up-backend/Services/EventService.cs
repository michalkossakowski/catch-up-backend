using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Database;
using catch_up_backend.Controllers;

public class EventService : IEventService
{
    private readonly CatchUpDbContext _context;
    private readonly EmailController emailController;

    public EventService(CatchUpDbContext context)
    {
        _context = context; 
        emailController = new EmailController();
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

    public async Task AddEventByPosition(Guid ownerId, string title, string description, string position, DateTime startDate, DateTime endDate)
    {
        var receivers = await _context.Users
            .Where(u => u.Position == position)
            .Select(u => u.Id)
            .ToListAsync();

        var eventEntry = new Event
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            OwnerId = ownerId,
            ReceiverIds = receivers
        };
        foreach(var receiver in receivers)
        {
            var newbie = await _context.Users.FindAsync(receiver);
            var sendNewbieEmailTask = Task.Run(() => emailController.SendEmail(newbie.Email,
            "Nowe Wydarzenie", $"Witaj {newbie.Name} {newbie.Surname}! \n W systemie zosta³o do Ciebie przydzielone nowe wydarzenie {title} maj¹ce miejsce w dniu {startDate.ToString()}. Koniecznie sprawdŸ szczegó³y wydarzenia na swoim profilu!"
            ));
        }
        _context.Events.Add(eventEntry);
        await _context.SaveChangesAsync();
    }

    public async Task AddEventByType(Guid ownerId, string title, string description, string type, DateTime startDate, DateTime endDate)
    {
        var receivers = await _context.Users
            .Where(u => u.Type == type)
            .Select(u => u.Id)
            .ToListAsync();

        var eventEntry = new Event
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            OwnerId = ownerId,
            ReceiverIds = receivers
        };

        _context.Events.Add(eventEntry);
        await _context.SaveChangesAsync();
    }

    public async Task AddEventForAllGroups(Guid ownerId, string title, string description, DateTime startDate, DateTime endDate)
    {
        var receivers = await _context.Users
            .Select(u => u.Id)
            .ToListAsync();

        var eventEntry = new Event
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            OwnerId = ownerId,
            ReceiverIds = receivers
        };

        _context.Events.Add(eventEntry);
        await _context.SaveChangesAsync();
    }
}