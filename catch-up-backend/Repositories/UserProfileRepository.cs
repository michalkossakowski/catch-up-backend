using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly CatchUpDbContext _context;

        public UserProfileRepository(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto> GetByUserIdAsync(Guid userId)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                return null;
            }

            var user = await _context.Users.FindAsync(userId);

            return MapToDto(profile, user);
        }

        public async Task<UserProfileDto> CreateProfileAsync(UserProfileDto userProfileDto)
        {
            var newProfile = new UserProfileModel
            {
                UserId = userProfileDto.UserId.Value,
                Bio = userProfileDto.Bio ?? string.Empty,
                Department = userProfileDto.Department ?? string.Empty,
                Location = userProfileDto.Location ?? string.Empty,
                Phone = userProfileDto.Phone ?? string.Empty,
                TeamsUsername = userProfileDto.TeamsUsername ?? string.Empty,
                SlackUsername = userProfileDto.SlackUsername ?? string.Empty,
                Interests = userProfileDto.Interests ?? new List<string>(),
                Languages = userProfileDto.Languages ?? new List<string>()
            };

            await _context.UserProfiles.AddAsync(newProfile);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userProfileDto.UserId);

            return MapToDto(newProfile, user);
        }

        public async Task<UserProfileDto> UpdateProfileAsync(Guid userId, UserProfileDto userProfileDto)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                userProfileDto.UserId = userId;
                return await CreateProfileAsync(userProfileDto);
            }

            if (userProfileDto.Bio != null)
                profile.Bio = userProfileDto.Bio;

            if (userProfileDto.Department != null)
                profile.Department = userProfileDto.Department;

            if (userProfileDto.Location != null)
                profile.Location = userProfileDto.Location;

            if (userProfileDto.Phone != null)
                profile.Phone = userProfileDto.Phone;

            if (userProfileDto.TeamsUsername != null)
                profile.TeamsUsername = userProfileDto.TeamsUsername;

            if (userProfileDto.SlackUsername != null)
                profile.SlackUsername = userProfileDto.SlackUsername;

            if (userProfileDto.Interests != null)
                profile.Interests = userProfileDto.Interests;

            if (userProfileDto.Languages != null)
                profile.Languages = userProfileDto.Languages;

            _context.UserProfiles.Update(profile);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userId);

            return MapToDto(profile, user);
        }

        private UserProfileDto MapToDto(UserProfileModel profile, UserModel user)
        {
            return new UserProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Bio = profile.Bio,
                Department = profile.Department,
                Location = profile.Location,
                Phone = profile.Phone,
                TeamsUsername = profile.TeamsUsername,
                SlackUsername = profile.SlackUsername,
                Interests = profile.Interests,
                Languages = profile.Languages
            };
        }
    }
} 