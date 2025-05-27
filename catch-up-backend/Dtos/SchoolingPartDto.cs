using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class SchoolingPartDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public FileDto IconFile { get; set; }
        public List<int> Materials { get; set; }
        public int schoolingUserId {get; set;}
    }
}
