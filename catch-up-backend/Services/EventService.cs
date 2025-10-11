using catch_up_backend.Constants;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

public class EventService : IEventService
{
    private readonly CatchUpDbContext _context;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;
    private readonly IUserService _userService;
    private readonly ILogger<EventService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EventService(
        CatchUpDbContext context, 
        IEmailService emailService,
        INotificationService notificationService,
        IUserService userService,
         IServiceProvider serviceProvider,
        ILogger<EventService> logger)
    {
        _context = context;
        _emailService = emailService;
        _notificationService = notificationService;
        _userService = userService;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<EventDto>> GetUserEvents(Guid userId)
    {
        var userRole = await _userService.GetRole(userId);

        var query = _context.Events.Where(e => e.State == StateEnum.Active && e.EndDate > DateTime.Now).AsQueryable();

        if (userRole != UserType.Admin)
        {
            query = query.Where(e => string.IsNullOrWhiteSpace(e.TargetUserType)
                || e.TargetUserType == userRole
                || e.OwnerId == userId);
        }

        return await query.Select(e => new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            OwnerId = e.OwnerId,
            TargetUserType = e.TargetUserType
        })
        .ToListAsync();
    }

    public async Task<bool> DeleteAsync(Guid userId, int eventId)
    {
        var userRole = await _userService.GetRole(userId);

        var eventToRemove = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == eventId 
                && (e.OwnerId == userId
                    || userRole == UserType.Admin));

        if (eventToRemove == null)
        {
            return false;
        }
        else
        {
            eventToRemove.State = StateEnum.Deleted;
            _context.Events.Update(eventToRemove);
            await _context.SaveChangesAsync();

            return true;
        }
    }

    public async Task<EventDto> AddAsync(EventDto eventDto)
    {
        var newEvent = new EventModel
        {
            Title = eventDto.Title,
            Description = eventDto.Description,
            StartDate = eventDto.StartDate,
            EndDate = eventDto.EndDate,
            OwnerId = eventDto.OwnerId,
            TargetUserType = eventDto.TargetUserType,
            State = StateEnum.Active
        };

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<EventService>>();

                await SendMailAndNotificationScoped(newEvent, userService, emailService, notificationService, logger);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Background task failed: {ex}");
            }
        });

        return new EventDto
        {
            Id = newEvent.Id,
            Title = newEvent.Title,
            Description = newEvent.Description,
            StartDate = newEvent.StartDate,
            EndDate = newEvent.EndDate,
            OwnerId = newEvent.OwnerId,
            TargetUserType = newEvent.TargetUserType
        };
    }

    private static async Task SendMailAndNotificationScoped(
        EventModel eventModel,
        IUserService userService,
        IEmailService emailService,
        INotificationService notificationService,
        ILogger logger)
    {
        var receivers = string.IsNullOrWhiteSpace(eventModel.TargetUserType) 
            ? await userService.GetQueryable().Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    Email = u.Email
                }).ToListAsync()
            : await userService.SearchUsersByRole(eventModel.TargetUserType);

        var subject = $"New Event: {eventModel.Title}";

        var body = $"Description: {eventModel.Description}\n" +
            $"Start: {eventModel.StartDate.ToString()}, End: {eventModel.EndDate.ToString()}";

        var receiverIds = receivers.Select(r => r.Id).ToList();
        try
        {
            var notificationReceiver = new NotificationModel(
                eventModel.OwnerId,
                subject,
                body,
                "/home"
            );

            await notificationService.AddNotification(notificationReceiver, receiverIds);
        }
        catch (Exception ex)
        {
            logger.LogError($"Cannot send notification to users {string.Join(", ", receiverIds)}: {ex.Message}");
        }

        //foreach (var receiver in receivers)
        //{
        //    try
        //    {
        //        await emailService.SendEmail(receiver.Email!, subject, body, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError($"Cannot send email to user {receiver.Id}: {ex.Message}");
        //    }
        //}
    }
}