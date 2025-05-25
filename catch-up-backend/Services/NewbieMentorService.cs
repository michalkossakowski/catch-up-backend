using catch_up_backend.Models;
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
    private readonly INotificationService _notificationService;

    public NewbieMentorService(CatchUpDbContext context, INotificationService notificationService)
    {
        _context = context;
        emailController = new EmailController();
        _notificationService = notificationService;
    }

    public async Task<bool> AssignNewbieToMentor(Guid newbieId, Guid mentorId)
    {
        UserModel? newbie = await _context.Users.FindAsync(newbieId);
        UserModel? mentor = await _context.Users.FindAsync(mentorId);
        if (newbie == null || mentor == null || newbie.Type != "Newbie" || mentor.Type != "Mentor")
        {
            return false;
        }

        NewbieMentorModel? assignment = await _context.NewbiesMentors.FindAsync(newbieId, mentorId);

        // Wysyłanie e-maili i powiadomień

        Task.Run(() => emailController.SendEmail(
            newbie.Email,
            "New Assignment",
            $"Hello {newbie.Name} {newbie.Surname}! \nA new mentor has been assigned to you in the system: {mentor.Name} {mentor.Surname}."
        ));
        Task.Run(() => emailController.SendEmail(
            mentor.Email,
            "New Assignment",
            $"Hello {mentor.Name} {mentor.Surname}! \nA new newbie has been assigned to you in the system: {newbie.Name} {newbie.Surname}."
        ));
        await _notificationService.AddNotification(new NotificationModel(
            newbieId,
            "New Assignment",
            $"Hello {newbie.Name} {newbie.Surname}! \nA new mentor has been assigned to you in the system: {mentor.Name} {mentor.Surname}.",
            $"/newbieMentor/profile/{newbie.Id}"
        ), newbieId);
        await _notificationService.AddNotification(new NotificationModel(
            mentorId,
            "New Assignment",
            $"Hello {mentor.Name} {mentor.Surname}! \nA new newbie has been assigned to you in the system: {newbie.Name} {newbie.Surname}.",
            $"/profile/{mentor.Id}"
        ), mentorId);



        if (assignment == null)
        {
            _context.NewbiesMentors.Add(new NewbieMentorModel(newbieId, mentorId));
        }
        else
        {
            if (assignment.State == StateEnum.Deleted)
            {
                assignment.StartDate = DateTime.Now;
            }
            assignment.EndDate = null;
            assignment.State = StateEnum.Active;
        }

        // Aktualizacja licznika odznak mentora
        mentor.Counters[BadgeTypeCountEnum.AssignNewbiesCount] = mentor.Counters.GetValueOrDefault(BadgeTypeCountEnum.AssignNewbiesCount, 0) + 1;
        _context.Users.Update(mentor);
        await _context.SaveChangesAsync();

        await new BadgeService(_context).AssignBadgeAutomatically(
            mentor.Id,
            BadgeTypeCountEnum.AssignNewbiesCount,
            mentor.Counters[BadgeTypeCountEnum.AssignNewbiesCount]
        );

        return true;
    }

    public async Task<bool> SetAssignmentState(Guid newbieId, Guid mentorId, StateEnum state)
    {
        NewbieMentorModel? assignment = await _context.NewbiesMentors.FindAsync(newbieId, mentorId);
        if (assignment == null)
        {
            return false;
        }

        UserModel? newbie = await _context.Users.FindAsync(newbieId);
        UserModel? mentor = await _context.Users.FindAsync(mentorId);

        await Task.WhenAll(
            Task.Run(() => emailController.SendEmail(
                newbie.Email,
                $"{state} Assign",
                $"Hello {newbie.Name} {newbie.Surname}! \n Mentor {mentor.Name} {mentor.Surname} has been unassign from you on system"
            )),
            Task.Run(() => emailController.SendEmail(
                mentor.Email,
                $"{state} Assign",
                $"Hello {mentor.Name} {mentor.Surname}! \n Newbie {newbie.Name} {newbie.Surname} has been unassign from you on system"
            ))
        );

        assignment.State = state;
        assignment.EndDate = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<UserModel>> GetAssignments(Guid id, RoleEnum role)
    {
        List<Guid> relatedIds;
        if (role == RoleEnum.Mentor)
        {
            relatedIds = await _context.NewbiesMentors
                .Where(a => a.State == StateEnum.Active && a.MentorId == id)
                .Select(a => a.NewbieId)
                .ToListAsync();
        }
        else
        {
            relatedIds = await _context.NewbiesMentors
                .Where(a => a.State == StateEnum.Active && a.NewbieId == id)
                .Select(a => a.MentorId)
                .ToListAsync();
        }

        var assignments = await _context.Users
            .Where(u => relatedIds.Contains(u.Id))
            .ToListAsync();

        return assignments;
    }

    public async Task<IEnumerable<UserModel>> GetUsers(RoleEnum role, bool? assigned, Guid? relatedId)
    {
        var query = _context.Users
        .Where(u => u.State == StateEnum.Active &&
        u.Type == role.ToString());


        if (assigned.HasValue)
        {
            if (assigned.Value)
            {
                query = query.Where(u => _context.NewbiesMentors
                    .Any(nm => nm.State == StateEnum.Active && (role == RoleEnum.Mentor ? nm.MentorId : nm.NewbieId) == u.Id));
            }
            else
            {
                if (relatedId.HasValue)
                {
                    query = query.Where(u => !_context.NewbiesMentors
                        .Any(nm => nm.State == StateEnum.Active && (role == RoleEnum.Mentor ? nm.MentorId : nm.NewbieId) == u.Id && (role == RoleEnum.Mentor ? nm.NewbieId : nm.MentorId) == relatedId));
                }
                else
                {
                    query = query.Where(u => !_context.NewbiesMentors
                        .Any(nm => nm.State == StateEnum.Active && (role == RoleEnum.Mentor ? nm.MentorId : nm.NewbieId) == u.Id));
                }
            }
        }

        var users = await query.ToListAsync();
        return users;
    }

    public async Task<IEnumerable<NewbieMentorModel>> GetAssignmentHistory(StateEnum? state)
    {
        var query = _context.NewbiesMentors.AsQueryable();
        if (state.HasValue)
        {
            query = query.Where(a => a.State == state.Value);
        }
        else
        {
            query = query.Where(a => a.State == StateEnum.Archived || a.State == StateEnum.Deleted);
        }

        return await query.ToListAsync();
    }
}