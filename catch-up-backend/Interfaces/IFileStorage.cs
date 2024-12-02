namespace catch_up_backend.Interfaces
{
    public interface IFileStorage
    {
        public Task<string> UploadFile(string fileName, Stream fileStream);
        public Task<Stream> DownloadFile(string source);
        public Task DeleteFile(string source);
    }
}
