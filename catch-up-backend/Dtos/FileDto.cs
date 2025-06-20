﻿
namespace catch_up_backend.Dtos
{
    public class FileDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Source { get; set; }
        public DateTime? DateOfUpload { get; set; }
        public long? SizeInBytes { get; set; }
        public Guid? Owner { get; set; }
    }
}
