using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catch_up_backend.Enums;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces
{
    public interface INewbieMentorService
    {
        Task<bool> AssignNewbieToMentor(Guid newbieId, Guid mentorId);
        Task<bool> Archive(Guid newbieId, Guid mentorId);
        Task<bool> Delete(Guid newbieId, Guid mentorId);
        Task<IEnumerable<UserModel>> GetAssignmentsByMentor(Guid mentorId);
        Task<int> GetNewbieCountByMentor(Guid mentorId);
        Task<IEnumerable<UserModel>> GetAssignmentsByNewbie(Guid newbieId);
        Task<int> GetMentorsCountByNewbie(Guid newbieId);
        Task<IEnumerable<NewbieMentorModel>> GetAllArchived();
        Task<IEnumerable<NewbieMentorModel>> GetAllDeleted();
        Task<IEnumerable<UserModel>> GetAllMentors();
        Task<IEnumerable<UserModel>> GetAllUnassignedNewbies(Guid mentorId);
        Task<string> GetDateStart(Guid newbieId, Guid mentorId);
        Task<string> GetDateEnd(Guid newbieId, Guid mentorId);
    }
}