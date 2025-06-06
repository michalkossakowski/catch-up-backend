using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(Guid userId);
        Task<UserProfileDto> CreateUserProfileAsync(UserProfileDto userProfileDto);
        Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UserProfileDto userProfileDto);
    }
} 