namespace catch_up_backend.Dtos
{
    public class TaskContentQueryParameters
    {
        private const int MaxPageSize = 300;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? TitleFilter { get; set; }
        public int? CategoryFilter { get; set; }
        public Guid? CreatorFilter { get; set; }
        public string? SortBy { get; set; } = "title";
        public string SortOrder { get; set; } = "asc";
    }
} 