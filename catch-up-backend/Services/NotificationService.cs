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
        private readonly IFirebaseService _firebaseService;

        public NotificationService(CatchUpDbContext context,
            INotificationHubService notificationHubService,
            IFirebaseService firebaseService)
        {
            _context = context;
            _notificationHubService = notificationHubService;
            _firebaseService = firebaseService;
        }
        public async Task<(List<NotificationDto>, int totalCount)> GetByUserId(Guid userId, int pageNumber = 1, int pageSize = 50)
        {
            var query = _context.UsersNotifications
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
            });

            var totalCount = await query.CountAsync();

            var notificationDtos = await query
                .OrderByDescending(n => n.SendDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (notificationDtos, totalCount);
        }
        public async Task ReadNotifications(Guid userId)
        {
            await _context.UsersNotifications
                .Where(un => un.State == StateEnum.Active && un.ReceiverId == userId && un.IsRead == false) 
                .ExecuteUpdateAsync(setters => setters.SetProperty(un => un.IsRead, true));

            await _context.SaveChangesAsync();
        }

        public async Task ReadNotification(Guid userId,int notificaitonId)
        {
            await _context.UsersNotifications
                .Where(un => un.State == StateEnum.Active && un.ReceiverId == userId && un.NotificationId == notificaitonId)
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
                try
                {
                    await _firebaseService.SendNotificationToUserAsync(
                        receiverId,
                        notificationDto.Title,
                        notificationDto.Message
                    );
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Cannot send firebase notificaiton");
                }

                try
                {
                    await _notificationHubService.SendNotification(receiverId, notificationDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot send singalR notificaiton");
                }

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

                    try
                    {
                        await _firebaseService.SendNotificationToUserAsync(
                            receiverId,
                            notificationDto.Title,
                            notificationDto.Message
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot send firebase notificaiton");
                    }

                    try
                    {
                        await _notificationHubService.SendNotification(receiverId, notificationDto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot send singalR notificaiton");
                    }
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
