using Azure.Core;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CatchUpDbContext _context;

        public UserRepository(CatchUpDbContext context){
            _context = context;
        }

        public async Task Add(UserDto newUser)
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
        }

        public async Task Edit(Guid userId, UserDto updatedUser)
        {
            var user = await _context.Users.FindAsync(userId);

            user.Name = updatedUser.Name;
            user.Surname = updatedUser.Surname;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            user.Type = updatedUser.Type;
            user.Position = updatedUser.Position;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            _context.Users.Remove(user);
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
    }
}
