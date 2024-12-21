using catch_up_backend.Dtos;
using catch_up_backend.Enums;

namespace catch_up_backend.Interfaces
{
    public interface ICompanySettingsService
    {
        Task<bool> UpdateSettings(Dictionary<string, bool> updatedSettings);
        Task<CompanySettingsDto> GetCompanySettings();

    }
}
