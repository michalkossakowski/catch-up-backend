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
        Task<bool> SetAssignmentState(Guid newbieId, Guid mentorId, StateEnum state);
        Task<IEnumerable<UserModel>> GetAssignments(Guid id, RoleEnum role);
        Task<IEnumerable<UserModel>> GetUsers(RoleEnum role, bool? assigned, Guid? relatedId);
        Task<IEnumerable<NewbieMentorModel>> GetAssignmentHistory(StateEnum? state);
    }
}