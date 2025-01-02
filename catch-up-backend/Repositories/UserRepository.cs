using Azure.Core;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;

namespace catch_up_backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CatchUpDbContext _context;

        public UserRepository(CatchUpDbContext context){
            _context = context;
        }

        public async Task<UserDto> Add(UserDto newUser)
        {
            var user = new UserModel(
                newUser.Name,
                newUser.Surname,
                newUser.Email,
                newUser.Password,
                newUser.Type,
                newUser.Position);

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Password = user.Password,
                Email = user.Email,
                Type = user.Type,
                Position = user.Position,
                Counters = user.Counters
            };
        }

        public async Task<UserDto> Edit(Guid userId, UserDto updatedUser)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return null;

            if (updatedUser.Name != null)
                user.Name = updatedUser.Name;

            if (updatedUser.Surname != null)
                user.Surname = updatedUser.Surname;

            if (updatedUser.Email != null)
                user.Email = updatedUser.Email;

            if (updatedUser.Password != null)
                user.Password = updatedUser.Password;

            if (updatedUser.Type != null)
                user.Type = updatedUser.Type;

            if (updatedUser.Position != null)
                user.Position = updatedUser.Position;

            if (updatedUser.AvatarId.HasValue)
                user.AvatarId = updatedUser.AvatarId;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Password = user.Password,
                Type = user.Type,
                Position = user.Position,
                AvatarId = user.AvatarId,
                Counters = user.Counters
            };
        }

        public async Task Delete(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            user.State = StateEnum.Deleted;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDto> GetById(Guid userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    Email = u.Email,
                    Position = u.Position,
                    AvatarId = u.AvatarId
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserModel> GetByMail(string email)
        {
            var user = await _context.Users
                .Where(u => u.Email == email).FirstOrDefaultAsync();

            return user;
        }

        public async Task<List<UserModel>> GetAll()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        public async Task<string> GetRole(Guid userId)
        {
            var userRole = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Type)
                .FirstOrDefaultAsync();

            return userRole;
        }
        public async Task<List<UserModel>> GetMentorAdmin()
        {
            return await _context.Users
            .Where(a => a.State == StateEnum.Active && (a.Type == "Mentor" || a.Type == "Admin"))
            .ToListAsync();
        }
    }
}
