using catch_up_backend.Models;

namespace catch_up_backend.Dtos
{
    public class SchoolingDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }

        public SchoolingDto() { }
        public SchoolingDto(SchoolingModel model) 
        { 
            Id = model.Id;
            CategoryId = model.CategoryId;
            CreatorId = model.CreatorId;
            Title = model.Title;
            Description = model.Description;
            Priority = model.Priority;
        }
    }

}
