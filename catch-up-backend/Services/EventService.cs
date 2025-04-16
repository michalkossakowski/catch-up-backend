using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Database;
using catch_up_backend.Controllers;
using catch_up_backend.Services;
using catch_up_backend.Interfaces;

public class EventService : IEventService
{
    private readonly CatchUpDbContext _context;
    private readonly EmailController emailController;
    private readonly INotificationService _notificationService;

    public EventService(CatchUpDbContext context, INotificationService notificationService)
    {
        _context = context; 
        emailController = new EmailController();
        _notificationService = notificationService;
    }

    public async Task<IEnumerable<EventModel>> GetUserEvents(Guid userId)
    {
        return await _context.Events
            .Where(e => e.OwnerId == userId || e.ReceiverIds.Contains(userId))
            .ToListAsync();
    }

    public async Task<IEnumerable<EventModel>> GetFullEvents()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task AddEventByPosition(Guid ownerId, string title, string description, string position, DateTime startDate, DateTime endDate)
    {
        var receivers = await _context.Users
            .Where(u => u.Position == position)
            .ToListAsync();
        var receiversIds = receivers.Select(r => r.Id).ToList();
        var eventEntry = new EventModel
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            OwnerId = ownerId,
            ReceiverIds = receiversIds
        };
        _context.Events.Add(eventEntry);
        SendMailAndNotification(receivers, eventEntry);
        await _context.SaveChangesAsync();
    }

    public async Task AddEventByType(Guid ownerId, string title, string description, string type, DateTime startDate, DateTime endDate)
    {
        List<UserModel> receivers = await _context.Users
            .Where(u => u.Type == type)
            .ToListAsync();
        var receiversIds = receivers.Select(r => r.Id).ToList();
        var eventEntry = new EventModel
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            OwnerId = ownerId,
            ReceiverIds = receiversIds
        };

        _context.Events.Add(eventEntry);
        SendMailAndNotification(receivers, eventEntry);
        await _context.SaveChangesAsync();
    }

    public async Task AddEventForAllGroups(Guid ownerId, string title, string description, DateTime startDate, DateTime endDate)
    {
        var receivers = await _context.Users
            .ToListAsync();
        var receiversIds = receivers.Select(r => r.Id).ToList();
        var eventEntry = new EventModel
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            OwnerId = ownerId,
            ReceiverIds = receiversIds
        };

        _context.Events.Add(eventEntry);
        await SendMailAndNotification(receivers, eventEntry);
        await _context.SaveChangesAsync();
    }
    private async Task SendMailAndNotification(List<UserModel> receivers, EventModel eventEntry)
    {
        foreach (UserModel receiver in receivers)
        {

            var sendMentorEmailTask = Task.Run(() => emailController.SendEmail(
                        receiver.Email,
                        "Nowe Przypisanie",
                                        $"Witaj {receiver.Name} {receiver.Surname}! \nW systemie zosta³o przypisane do Ciebie nowe wydarzenie {eventEntry.Title} maj¹ce miejsce {eventEntry.StartDate.ToString()} - {eventEntry.EndDate.ToString()}!\n Opis wydarzenia: {eventEntry.Description}"
                    ));
            var notificationReceiver = new NotificationModel(
                receiver.Id,
                "Nowe Wydarzenie",
                $"Witaj {receiver.Name} {receiver.Surname}! \nW systemie zosta³o przypisane do Ciebie nowe wydarzenie {eventEntry.Title} maj¹ce miejsce {eventEntry.StartDate.ToString()} - {eventEntry.EndDate.ToString()}!\n Opis wydarzenia: {eventEntry.Description}",
                "kot"
            );
            await _notificationService.AddNotification(notificationReceiver, receiver.Id);
        }
    }
}