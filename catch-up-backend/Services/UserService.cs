using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class UserService : IUserService{
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository){
            this.userRepository = userRepository;
        }

        public async Task<UserDto> Add(UserDto newUser)
        {
            return await userRepository.Add(newUser);
        }

        public async Task Edit(Guid userId, UserDto updatedUser){
            await userRepository.Edit(userId, updatedUser);
        }
        
        public async Task Delete(Guid userId){
            await userRepository.Delete(userId);
        }

        public async Task<UserDto> GetById(Guid userId){
            return await userRepository.GetById(userId);
        }

        public async Task<List<UserModel>> GetAll(){
            return await userRepository.GetAll();
        }

        public async Task<string> GetRole(Guid userId)
        {
            return await userRepository.GetRole(userId);
        }
        public async Task<List<UserModel>> GetMentorAdmin()
        {
            return await userRepository.GetMentorAdmin();
        }

        public async Task<IEnumerable<UserDto>> SearchUsers(string searchPhrase)
        {
            return await userRepository.SearchUsers(searchPhrase);
        }

        public async Task<IEnumerable<UserDto>> SearchUsersByRole(string role, string? searchPhrase = null)
        {
            return await userRepository.SearchUsersByRole(role, searchPhrase);
        }
    }
}
