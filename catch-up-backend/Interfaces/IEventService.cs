using catch_up_backend.Dtos;

public interface IEventService
{
    Task<IEnumerable<EventDto>> GetUserEvents(Guid userId);
    Task<EventDto> AddAsync(EventDto eventDto);
    Task<bool> DeleteAsync(Guid userId, int eventId);
}