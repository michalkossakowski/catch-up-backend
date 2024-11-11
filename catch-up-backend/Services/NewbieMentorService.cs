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

    public async Task AssignNewbieToMentor(NewbieMentorModel assignment)
    {
        _context.NewbiesMentors.Add(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EditAssignment(Guid newbieId, Guid mentorId, NewbieMentorModel updatedAssignment)
    {
        NewbieMentorModel? assignment = await _context.NewbiesMentors
            .FindAsync(newbieId, mentorId);

        if (assignment == null)
        {
            return false;
        }

        assignment.IsActive = updatedAssignment.IsActive;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> ChangeIsActive(Guid newbieId, Guid mentorId, bool status)
    {
        NewbieMentorModel? assignment = await _context.NewbiesMentors
            .FindAsync(newbieId, mentorId);

        if (assignment == null)
        {
            return false;
        }

        assignment.IsActive = status;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> ChangeState(Guid newbieId, Guid mentorId, StateEnum newState)
    {
        NewbieMentorModel? connection = await _context.NewbiesMentors
                                       .FirstOrDefaultAsync(nm => nm.NewbieId == newbieId && nm.MentorId == mentorId);

        if (connection == null)
        {
            return false;
        }

        connection.State = newState;
        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> UnassignNewbieFromMentor(Guid newbieId,Guid mentorId)
    {
        NewbieMentorModel? assignment = await _context.NewbiesMentors
            .FindAsync(newbieId, mentorId); //sprawdza czy jest połączenie między mentorem, a newbie

        if (assignment == null)
        {
            return false;
        }

        _context.NewbiesMentors.Remove(assignment);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<NewbieMentorModel>> GetAssignmentsByMentor(Guid mentorId)
    {
        return await _context.NewbiesMentors
            .Where(a =>a.State==StateEnum.Active && a.IsActive==true && a.MentorId == mentorId && a.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<NewbieMentorModel>> GetAssignmentsByNewbie(Guid newbieId)
    {
        return await _context.NewbiesMentors
            .Where(a => a.State == StateEnum.Active && a.IsActive == true && a.NewbieId == newbieId && a.IsActive)
            .ToListAsync();
    }
    public async Task<bool> GetIsActive(Guid newbieId, Guid mentorId)
    {
        NewbieMentorModel? assignment = await _context.NewbiesMentors
             .FindAsync(newbieId, mentorId);
        if (assignment == null)
        {
            return false;
        }
        return assignment.IsActive;
    }
    public async Task<StateEnum?> GetState(Guid newbieId, Guid mentorId)
    {
        NewbieMentorModel? assignment = await _context.NewbiesMentors
                                      .FirstOrDefaultAsync(nm => nm.NewbieId == newbieId && nm.MentorId == mentorId);
        return assignment?.State;
    }
}