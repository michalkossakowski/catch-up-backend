﻿using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;
using catch_up_backend.Services;
using catch_up_backend.Controllers;

public class NewbieMentorService : INewbieMentorService
{
    private readonly CatchUpDbContext _context;
    private readonly EmailController emailController;
    public NewbieMentorService(CatchUpDbContext context)
    {
        _context = context;
        emailController = new EmailController();
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
                var sendNewbieEmailTask = Task.Run(() => emailController.SendEmail(
                newbie.Email,
                "Nowe Przypisanie",
                $"Witaj {newbie.Name} {newbie.Surname}! \nW systemie został Ci przypisany nowy mentor {mentor.Name} {mentor.Surname}"
                ));

                var sendMentorEmailTask = Task.Run(() => emailController.SendEmail(
                    mentor.Email,
                    "Nowe Przypisanie",
                    $"Witaj {mentor.Name} {mentor.Surname}! \nW systemie został Ci przypisany nowy newbie {newbie.Name} {newbie.Surname}"
                ));
                if (assignment == null)
                {
                    NewbieMentorModel newAssignment = new NewbieMentorModel(newbieId, mentorId);
                    _context.NewbiesMentors.Add(newAssignment);
                }
                else
                {
                    if (assignment.State == StateEnum.Deleted) //jeżeli jest usunięty to zliczanie od początku liczymy, jak zaarchiwizowany to usuwamy tylko końcową, jakby kontynuacja
                    {
                        assignment.StartDate = DateTime.Now;
                    }
                    assignment.EndDate = null;
                    assignment.State = StateEnum.Active;
                }
                if (mentor.Counters.ContainsKey(BadgeTypeCountEnum.AssignNewbiesCount))
                {
                    mentor.Counters[BadgeTypeCountEnum.AssignNewbiesCount]++;
                }
                else
                {
                    mentor.Counters[BadgeTypeCountEnum.AssignNewbiesCount] = 1;
                }

                _context.Users.Update(mentor);
                await _context.SaveChangesAsync();
                await new BadgeService(_context).AssignBadgeAutomatically(
                mentor.Id,
                BadgeTypeCountEnum.AssignNewbiesCount,
                mentor.Counters[BadgeTypeCountEnum.AssignNewbiesCount]
            );
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
        UserModel? newbie = await _context.Users.FindAsync(newbieId);
        UserModel? mentor = await _context.Users.FindAsync(mentorId);
        var sendNewbieEmailTask = Task.Run(() => emailController.SendEmail(newbie.Email,
            "Archiwizacja Przypisania", $"Witaj {newbie.Name} {newbie.Surname}! \n W systemie mentor {mentor.Name} {mentor.Surname} został od Ciebie odpięty"
            ));
        var sendMentorEmailTask = Task.Run(() => emailController.SendEmail(mentor.Email,
         "Archiwizacja Przypisania", $"Witaj {mentor.Name} {mentor.Surname}! \n W systemie newbie {newbie.Name} {newbie.Surname} został od Ciebie odpięty"
         ));
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
        UserModel? newbie = await _context.Users.FindAsync(newbieId);
        UserModel? mentor = await _context.Users.FindAsync(mentorId);
        var sendNewbieEmailTask = Task.Run(() => emailController.SendEmail(newbie.Email,
            "Usunięcie Przypisania", $"Witaj {newbie.Name} {newbie.Surname}! \n W systemie mentor {mentor.Name} {mentor.Surname} został od Ciebie odpięty"
            ));
        var sendMentorEmailTask = Task.Run(() => emailController.SendEmail(mentor.Email,
         "Usunięcie Przypisania", $"Witaj {mentor.Name} {mentor.Surname}! \n W systemie newbie {newbie.Name} {newbie.Surname} został od Ciebie odpięty"
         ));
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
    public async Task<IEnumerable<UserModel>> GetAllNewbies()
    {
        return await _context.Users
            .Where(a => a.State == StateEnum.Active && a.Type == "Newbie")
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
    public async Task<IEnumerable<UserModel>> GetAllUnassignedMentors(Guid newbieId)
    {
        return await _context.Users
           .Where(user =>
               user.State == StateEnum.Active &&
               user.Type == "Mentor" &&
               !_context.NewbiesMentors
                   .Any(nm => nm.State == StateEnum.Active && nm.NewbieId == newbieId && nm.MentorId == user.Id))
           .ToListAsync();
    }
    public async Task<IEnumerable<UserModel>> GetAllUnassignedNewbies()
    {
        return await _context.Users
            .Where(user =>
                user.State == StateEnum.Active &&
                user.Type == "Newbie" &&
                !_context.NewbiesMentors
                    .Any(nm => nm.State == StateEnum.Active && nm.NewbieId == user.Id))
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