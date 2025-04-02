namespace catch_up_backend.Interfaces
{
    public interface IFirebaseService
    {
        Task<bool> RegisterAsync(Guid userId, string firebaseToken, string deviceName);
        Task<bool> UnregisterAsync(Guid userId, string firebaseToken);
        Task SendNotificationToUserAsync(Guid userId, string title, string body);
        Task<string> GetFirebaseTokenByUserIdAsync(Guid userId);
    }
}
