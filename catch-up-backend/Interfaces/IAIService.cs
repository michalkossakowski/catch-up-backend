using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IAIService
    {
        Task<string> GenerateAIChatResponse(AIChatDto aIChatDto);
    }
}
