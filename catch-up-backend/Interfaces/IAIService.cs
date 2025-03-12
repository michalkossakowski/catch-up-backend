namespace catch_up_backend.Interfaces
{
    public interface IAIService
    {
        Task<string> GenerateAIChatResponse(string message);
    }
}
