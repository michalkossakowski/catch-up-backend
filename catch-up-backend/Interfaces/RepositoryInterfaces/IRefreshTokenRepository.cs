using catch_up_backend.Models;

namespace catch_up_backend.Interfaces.RepositoryInterfaces
{
    public interface IRefreshTokenRepository{
        Task Add(RefreshTokenModel refreshToken);
        Task<RefreshTokenModel> GetByUserId(Guid userId);
        Task<RefreshTokenModel> DoesTokenExist(string refreshToken, Guid userId);
        Task Delete(RefreshTokenModel refreshToken);
    }
}
