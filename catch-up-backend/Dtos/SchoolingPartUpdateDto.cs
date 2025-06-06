namespace catch_up_backend.Dtos
{
    public class SchoolingPartUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public int IconFileID { get; set; }
        public List<int> MaterialsId { get; set; }

    }
}
