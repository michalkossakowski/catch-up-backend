using Azure.Core;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly CatchUpDbContext _context;
        private readonly INotificationHubService _notificationHubService;

        public NotificationService(CatchUpDbContext context, INotificationHubService notificationHubService)
        {
            _context = context;
            _notificationHubService = notificationHubService;
        }
        public async Task<List<NotificationDto>> GetByUserId(Guid userId)
        {
            var notificationDtos = await _context.UsersNotifications
                .Where(un => un.State == StateEnum.Active && un.ReceiverId == userId)
                .Join(_context.Notifications,
                    un => un.NotificationId,
                    n => n.Id,
                    (un, n) => new NotificationDto
                    {
                        NotificationId = n.Id,
                        SenderId = n.SenderId,
                        ReceiverId = userId,
                        Title = n.Title,
                        Message = n.Message,
                        SendDate = n.SendDate,
                        Source = n.Source,
                        IsRead = un.IsRead
                    })
                .OrderByDescending(n => n.SendDate)
                .ToListAsync();

            return notificationDtos;
        }
        public async Task ReadNotifications(Guid userId)
        {
            await _context.UsersNotifications
                .Where(un => un.State == StateEnum.Active && un.ReceiverId == userId && un.IsRead == false) 
                .ExecuteUpdateAsync(setters => setters.SetProperty(un => un.IsRead, true));

            await _context.SaveChangesAsync();
        }
        public async Task<bool> HasUnreadNotifications(Guid userId)
        {
            var userNotifications = await _context.UsersNotifications
                .Where(un => un.State == StateEnum.Active && un.ReceiverId == userId && un.IsRead == false)
                .ToListAsync();

            return userNotifications.Any();
        }
        public async Task AddNotification(NotificationModel notification, Guid receiverId)
        {
            try
            {
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                var notificationDto = new NotificationDto
                {
                    NotificationId = notification.Id,
                    SenderId = notification.SenderId,
                    ReceiverId = Guid.Empty,
                    Title = notification.Title,
                    Message = notification.Message,
                    SendDate = notification.SendDate,
                    Source = notification.Source,
                    IsRead = false
                };


                var userNotification = new UserNotificationModel(notification.Id, receiverId);
                await _context.AddAsync(userNotification);

                notificationDto.ReceiverId = receiverId;
                await _notificationHubService.SendNotification(receiverId, notificationDto);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Notification Add: " + ex);
            }
        }

        public async Task AddNotification(NotificationModel notification, List<Guid> receiverIds)
        {
            try
            {
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                var notificationDto = new NotificationDto
                {
                    NotificationId = notification.Id,
                    SenderId = notification.SenderId,
                    ReceiverId = Guid.Empty,
                    Title = notification.Title,
                    Message = notification.Message,
                    SendDate = notification.SendDate,
                    Source = notification.Source,
                    IsRead = false
                };


                var userNotifications = new List<UserNotificationModel>();

                foreach (var receiverId in receiverIds)
                {
                    var userNotification = new UserNotificationModel(notification.Id, receiverId);
                    userNotifications.Add(userNotification);

                    notificationDto.ReceiverId = receiverId;
                    await _notificationHubService.SendNotification(receiverId, notificationDto);
                }

                await _context.AddRangeAsync(userNotifications);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Notification Add: " + ex);
            }
        }
    }
}
