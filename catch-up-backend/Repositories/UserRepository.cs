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
                    Position = u.Position
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

        public async Task<IEnumerable<UserDto>> SearchUsers(string searchPhrase)
        {
            if (string.IsNullOrWhiteSpace(searchPhrase))
                return await _context.Users
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        Email = u.Email,
                        Type = u.Type,
                        Position = u.Position
                    })
                    .ToListAsync();

            searchPhrase = searchPhrase.ToLower();
            return await _context.Users
                .Where(u => u.Name.ToLower().Contains(searchPhrase) ||
                            u.Surname.ToLower().Contains(searchPhrase) ||
                            u.Email.ToLower().Contains(searchPhrase))
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    Email = u.Email,
                    Type = u.Type,
                    Position = u.Position
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> SearchUsersByRole(string role, string? searchPhrase = null)
        {
            var query = _context.Users.Where(u => u.Type.ToLower() == role.ToLower());

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                searchPhrase = searchPhrase.ToLower();
                query = query.Where(u => u.Name.ToLower().Contains(searchPhrase) ||
                                        u.Surname.ToLower().Contains(searchPhrase) ||
                                        u.Email.ToLower().Contains(searchPhrase));
            }

            return await query
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    Email = u.Email,
                    Type = u.Type,
                    Position = u.Position
                })
                .ToListAsync();
        }
    }
}
