using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using catch_up_backend.Models;

public interface IEventService
{
    Task<IEnumerable<Event>> GetUserEvents(Guid userId);
    Task<IEnumerable<Event>> GetFullEvents();
    Task AddEventByPosition(Guid ownerId, string title, string position, DateTime startDate, DateTime endDate);
    Task AddEventByType(Guid ownerId, string title, string type, DateTime startDate, DateTime endDate);
    Task AddEventForAllGroups(Guid ownerId, string title, DateTime startDate, DateTime endDate);
}