using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class CompanySettingsDto
    {
        public int Id { get; set; }
        public Dictionary<string, bool> Settings { get; set; } = new Dictionary<string, bool>();//pierwszy wyraz to opcja, druga to jej stan
    }
}
