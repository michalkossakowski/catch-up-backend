using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;

public class NewbieMentorService : INewbieMentorService
{
    private readonly CatchUpDbContext _context;

    public NewbieMentorService(CatchUpDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AssignNewbieToMentor(Guid newbieId, Guid mentorId)
    {
        UserModel? newbie = _context.Users.FindAsync(newbieId).Result;
        UserModel? mentor = _context.Users.FindAsync(mentorId).Result;
        if (newbie != null && mentor != null)
        {
            string? newbieType = newbie?.Type;
            string? mentorType = mentor?.Type;

            if (newbieType == "Newbie" && mentorType == "Mentor")
            {
                NewbieMentorModel? assignment = await _context.NewbiesMentors
                    .FindAsync(newbieId, mentorId);

                if (assignment == null)
                {
                    NewbieMentorModel newAssignment = new NewbieMentorModel(newbieId, mentorId);
                    _context.NewbiesMentors.Add(newAssignment);
                }
                else
                {
                    assignment.State = StateEnum.Active;
                    assignment.EndDate = null;
                }
                await _context.SaveChangesAsync();
                return true;
            }
        }
        return false;
    }
    public async Task<bool> Archive(Guid newbieId, Guid mentorId)
    {
        NewbieMentorModel? assignment = await _context.NewbiesMentors
            .FindAsync(newbieId, mentorId); 

        if (assignment == null)
        {
            return false;
        }

        assignment.State = StateEnum.Archived;
        assignment.EndDate = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> Delete(Guid newbieId,Guid mentorId)
    {
        NewbieMentorModel? assignment = await _context.NewbiesMentors
            .FindAsync(newbieId, mentorId); 

        if (assignment == null)
        {
            return false;
        }

        assignment.State= StateEnum.Deleted;
        assignment.EndDate = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<UserModel>> GetAssignmentsByMentor(Guid mentorId)
    {
        List<Guid> newbieIds = await _context.NewbiesMentors
            .Where(a => a.State == StateEnum.Active && a.MentorId == mentorId)
            .Select(a => a.NewbieId) 
            .ToListAsync();

        return await _context.Users
            .Where(u => newbieIds.Contains(u.Id)) 
            .ToListAsync();
    }
    public async Task<int> GetNewbieCountByMentor(Guid mentorId)
    {
        return await _context.NewbiesMentors
            .Where(a => a.State == StateEnum.Active && a.MentorId == mentorId)
            .CountAsync();
    }

    public async Task<IEnumerable<UserModel>> GetAssignmentsByNewbie(Guid newbieId)
    {
        List<Guid> mentorIds = await _context.NewbiesMentors
           .Where(a => a.State == StateEnum.Active && a.NewbieId == newbieId)
           .Select(a => a.MentorId)
           .ToListAsync();

        return await _context.Users
            .Where(u => mentorIds.Contains(u.Id))
            .ToListAsync();
    }
    public async Task<int> GetMentorsCountByNewbie(Guid newbieId)
    {
        return await _context.NewbiesMentors
           .Where(a => a.State == StateEnum.Active && a.NewbieId == newbieId)
           .CountAsync();
    }
    public async Task<IEnumerable<NewbieMentorModel>> GetAllArchived()
    {
        return await _context.NewbiesMentors
           .Where(a => a.State == StateEnum.Archived)
           .ToListAsync();
    }
    public async Task<IEnumerable<NewbieMentorModel>> GetAllDeleted()
    {
        return await _context.NewbiesMentors
           .Where(a => a.State == StateEnum.Deleted)
           .ToListAsync();
    }
    public async Task<IEnumerable<UserModel>> GetAllMentors()
    {
        return await _context.Users
            .Where(a => a.State == StateEnum.Active && a.Type == "Mentor")
            .ToListAsync();
    }
    public async Task<IEnumerable<UserModel>> GetAllUnassignedNewbies(Guid mentorId)
    {
        return await _context.Users
            .Where(user =>
                user.State == StateEnum.Active &&
                user.Type == "Newbie" &&
                !_context.NewbiesMentors
                    .Any(nm => nm.State == StateEnum.Active && nm.NewbieId == user.Id && nm.MentorId == mentorId))
            .ToListAsync();
    }
    public async Task<string> GetDateStart(Guid newbieId, Guid mentorId)
    {
         NewbieMentorModel? newbieMentor= await _context.NewbiesMentors
             .FindAsync(newbieId, mentorId);
        if(newbieMentor == null)
        {
            return null;
        }
        return newbieMentor.StartDate.ToString();
    }
    public async Task<string> GetDateEnd(Guid newbieId, Guid mentorId)
    {
        NewbieMentorModel? newbieMentor = await _context.NewbiesMentors
            .FindAsync(newbieId, mentorId);
        if (newbieMentor == null)
        {
            return null;
        }
        return newbieMentor.EndDate.ToString();
    }
}