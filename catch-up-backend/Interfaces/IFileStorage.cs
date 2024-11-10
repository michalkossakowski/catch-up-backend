namespace catch_up_backend.Interfaces
{
    public interface IFileStorage
    {
        public Task<string> UploadFileAsync(string fileName, Stream fileStream);
        public Task<Stream> DownloadFileAsync(string source);
        public Task DeleteFileAsync(string source);
    }
}
