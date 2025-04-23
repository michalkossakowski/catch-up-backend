﻿using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces
{
    public interface IUserService{
        Task<UserDto> Add(UserDto newUser);
        Task<UserDto> Edit(Guid userId, UserDto updatedUser);
        Task Delete(Guid userId);
        Task<UserDto> GetById(Guid userId);
        Task<List<UserModel>> GetAll();
        Task<List<UserModel>> GetMyNewbies(Guid mentorId);
        Task<string> GetRole(Guid userId);
        Task<List<UserModel>> GetMentorAdmin();
        Task<IEnumerable<UserDto>> SearchUsers(string searchPhrase);
        Task<IEnumerable<UserDto>> SearchUsersByRole(string role, string? searchPhrase = null);
    }
}