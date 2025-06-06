using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces.RepositoryInterfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfileDto> GetByUserIdAsync(Guid userId);
        Task<UserProfileDto> CreateProfileAsync(UserProfileDto userProfileDto);
        Task<UserProfileDto> UpdateProfileAsync(Guid userId, UserProfileDto userProfileDto);
    }
} 