using catch_up_backend.Database;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly CatchUpDbContext _context;

        public FirebaseService(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(Guid userId, string firebaseToken, string deviceName)
        {
            try
            {
                var existingToken = await _context.FirebaseTokens
                    .FirstOrDefaultAsync(ft => ft.UserId == userId && ft.FirebaseToken == firebaseToken);

                if (existingToken != null)
                {
                    return true;
                }

                var existingDeviceToken = await _context.FirebaseTokens
                    .FirstOrDefaultAsync(ft => ft.FirebaseToken == firebaseToken);

                if (existingDeviceToken != null)
                {
                    _context.FirebaseTokens.Remove(existingDeviceToken);
                }

                var firebaseTokenModel = new FirebaseTokenModel(userId, firebaseToken, deviceName);
                await _context.AddAsync(firebaseTokenModel);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding Firebase Token: " + ex);
            }
        }

        public async Task<string> GetFirebaseTokenByUserIdAsync(Guid userId)
        {
            var firebaseTokenModel = await _context.FirebaseTokens.FindAsync(userId);
            return firebaseTokenModel?.FirebaseToken ?? string.Empty;
        }

        public async Task<bool> UnregisterAsync(Guid userId, string firebaseToken)
        {
            try
            {
                var existingToken = await _context.FirebaseTokens
                    .FirstOrDefaultAsync(ft => ft.FirebaseToken == firebaseToken);

                if (existingToken == null)
                {
                    return true;
                }
                _context.FirebaseTokens.Remove(existingToken);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while removing Firebase Token: " + ex);
            }
        }

        public async Task SendNotificationToUserAsync(Guid userId, string title, string body)
        {
            var tokens = await _context.FirebaseTokens
                .Where(ft => ft.UserId == userId)
                .Select(ft => ft.FirebaseToken)
                .ToListAsync();

            if (tokens == null || !tokens.Any())
            {
                throw new Exception("No Firebase tokens found for this user.");
            }

            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        ChannelId = "com.unhandledexception.catchupmobile.general",
                        Sound = "default",
                        Priority = NotificationPriority.MAX,
                        Visibility = NotificationVisibility.PUBLIC,
                    }
                }
            };

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

                Console.WriteLine("Notification sent successfully: " + response);

                if (response.FailureCount > 0)
                {
                    var failedTokens = response.Responses
                        .Select((r, i) => new { Response = r, Token = tokens[i] })
                        .Where(x => !x.Response.IsSuccess)
                        .Select(x => x.Token)
                        .ToList();

                    await RemoveInvalidTokens(failedTokens);
                }
            }
            catch (FirebaseMessagingException fex)
            {
                throw new Exception($"FirebaseMessagingException: {fex.Message}, ErrorCode: {fex.ErrorCode}, HttpStatus: {fex.HttpResponse?.StatusCode}");
            }
        }

        private async Task RemoveInvalidTokens(List<string> invalidTokens)
        {
            var tokensToRemove = await _context.FirebaseTokens
                .Where(ft => invalidTokens.Contains(ft.FirebaseToken))
                .ToListAsync();

            _context.FirebaseTokens.RemoveRange(tokensToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
