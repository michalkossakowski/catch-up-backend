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
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<NewbieMentorModel>> GetAssignmentsByMentor(Guid mentorId)
    {
        return await _context.NewbiesMentors
            .Where(a =>a.State==StateEnum.Active && a.MentorId == mentorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<NewbieMentorModel>> GetAssignmentsByNewbie(Guid newbieId)
    {
        return await _context.NewbiesMentors
            .Where(a => a.State == StateEnum.Active && a.NewbieId == newbieId)
            .ToListAsync();
    }
    public async Task<StateEnum?> GetState(Guid newbieId, Guid mentorId)
    {
        NewbieMentorModel? assignment = await _context.NewbiesMentors
                                      .FirstOrDefaultAsync(nm => nm.NewbieId == newbieId && nm.MentorId == mentorId);
        return assignment?.State;
    }
}