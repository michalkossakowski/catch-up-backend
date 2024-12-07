using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces
{
    public interface IUserService{
        Task Add(UserDto newUser);
        Task Edit(Guid userId, UserDto updatedUser);
        Task Delete(Guid userId);
        Task<UserDto> GetById(Guid userId);
        Task<List<UserModel>> GetAll();
        Task<string> GetRole(Guid userId);
        Task<List<UserModel>> GetMentorAdmin();
    }
}