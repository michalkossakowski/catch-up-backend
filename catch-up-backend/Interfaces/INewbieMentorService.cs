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
        Task<IEnumerable<NewbieMentorModel>> GetAssignmentsByMentor(Guid mentorId);
        Task<IEnumerable<NewbieMentorModel>> GetAssignmentsByNewbie(Guid newbieId);
    }
}