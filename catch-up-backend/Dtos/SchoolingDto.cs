using catch_up_backend.Models;

namespace catch_up_backend.Dtos
{
    public class SchoolingDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public int Priority { get; set; }
        public FileDto IconFile { get; set; }
        public string Content { get; set; }
        public List<SchoolingPartProgressBarDto> SchoolingPartProgressBar{ get; set; }
        public SchoolingDto() { }
        public SchoolingDto(SchoolingModel model)
        {
            Id = model.Id;
            CategoryId = model.CategoryId;
            CreatorId = model.CreatorId;
            Title = model.Title;
            ShortDescription = model.ShortDescription;
            Priority = model.Priority;
            Content = model.Content;
        }
        public SchoolingDto(SchoolingModel model, FileDto iconFileDto)
        {
            Id = model.Id;
            CategoryId = model.CategoryId;
            CreatorId = model.CreatorId;
            Title = model.Title;
            ShortDescription = model.ShortDescription;
            Priority = model.Priority;
            Content = model.Content;
            IconFile = iconFileDto;
        }
    }

}
