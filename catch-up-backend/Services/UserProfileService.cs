using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using System.Text.RegularExpressions;

namespace catch_up_backend.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            return await _userProfileRepository.GetByUserIdAsync(userId);
        }

        public async Task<UserProfileDto> CreateUserProfileAsync(UserProfileDto userProfileDto)
        {
            ValidateUserProfile(userProfileDto);
            return await _userProfileRepository.CreateProfileAsync(userProfileDto);
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UserProfileDto userProfileDto)
        {
            ValidateUserProfile(userProfileDto);
            return await _userProfileRepository.UpdateProfileAsync(userId, userProfileDto);
        }
        
        private void ValidateUserProfile(UserProfileDto userProfileDto)
        {
            if (!string.IsNullOrEmpty(userProfileDto.Phone))
            {
                string digitsOnly = Regex.Replace(userProfileDto.Phone, @"\D", "");
                
                if (digitsOnly.Length < 6 || digitsOnly.Length > 15)
                {
                    throw new ArgumentException("Invalid phone number", "Phone");
                }
            }
        }
    }
} 