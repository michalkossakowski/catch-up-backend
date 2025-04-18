namespace catch_up_backend.Dtos
{
    public class SchoolingPartProgressBarDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public FileDto FileIcon { get; set; }
        public bool IsDone { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }

        public SchoolingPartProgressBarDto() { }

    }
}
