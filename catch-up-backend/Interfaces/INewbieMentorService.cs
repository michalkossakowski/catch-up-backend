using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catch_up_backend.Enums;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces
{
    public interface INewbieMentorService
    {
        Task AssignNewbieToMentor(NewbieMentorModel assignment);
        Task<bool> EditAssignment(Guid newbieId, Guid mentorId, NewbieMentorModel updatedAssignment);
        Task<bool> ChangeIsActive(Guid newbieId, Guid mentorId, bool status);
        Task<bool> ChangeState(Guid newbieId, Guid mentorId, StateEnum newState);
        Task<bool> UnassignNewbieFromMentor(Guid newbieId, Guid mentorId);
        Task<IEnumerable<NewbieMentorModel>> GetAssignmentsByMentor(Guid mentorId);
        Task<IEnumerable<NewbieMentorModel>> GetAssignmentsByNewbie(Guid newbieId);
        Task<bool> GetIsActive(Guid newbieId, Guid mentorId);
        Task<StateEnum?> GetState(Guid newbieId, Guid mentorId);
    }
}