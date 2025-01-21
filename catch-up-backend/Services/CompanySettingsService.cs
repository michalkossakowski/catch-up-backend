using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime;

namespace catch_up_backend.Services
{
    public class CompanySettingsService : ICompanySettingsService
    {
        private readonly CatchUpDbContext _context;
        public CompanySettingsService(CatchUpDbContext context)
        {
            _context = context;

            if (!_context.CompanySettings.Any())
            {
                var defaultSettings = new List<SettingModel>
                {
                    new SettingModel("IsLocalizationRestricted")
                };
                _context.CompanySettings.AddRange(defaultSettings);
                _context.SaveChanges();
            }
        }
        public async Task<bool> UpdateSettings(Dictionary<string, bool> updatedSettings)
        {
            var companySettings = await _context.CompanySettings.ToListAsync();
            foreach (var setting in updatedSettings)
            {
                var existingSetting = companySettings.FirstOrDefault(s=>s.Name == setting.Key);
                if (existingSetting != null)
                {
                    if (existingSetting.Value != setting.Value)
                    {
                        existingSetting.Value = setting.Value;
                        _context.CompanySettings.Update(existingSetting);
                    }
                }
                else
                {
                    return false;
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<CompanySettingsDto> GetCompanySettings()
        {
            var companySettings = await _context.CompanySettings.ToListAsync();

            CompanySettingsDto dto = new CompanySettingsDto
            {
                Settings = companySettings.ToDictionary(s => s.Name, s => s.Value)
            };
            return dto;
        }
        public async Task<bool> TurnOnLocalization()
        {
            SettingModel companySetting = await _context.CompanySettings.FirstOrDefaultAsync(s => s.Name == "IsLocalizationRestricted");
            companySetting.Value = true;
            _context.SaveChanges();
            return companySetting.Value;
        }
        public async Task<bool> TurnOffLocalization()
        {
            SettingModel companySetting = await _context.CompanySettings.FirstOrDefaultAsync(s => s.Name == "IsLocalizationRestricted");
            companySetting.Value = false;
            _context.SaveChanges();
            return companySetting.Value;
        }
    }
}
