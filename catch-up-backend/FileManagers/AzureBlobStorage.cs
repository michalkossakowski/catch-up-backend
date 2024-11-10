using catch_up_backend.Interfaces;

namespace catch_up_backend.FileManagers
{
    public class AzureBlobStorage : IFileStorage
    {
        private string? v;

        public AzureBlobStorage(string? v, string v1)
        {
            this.v = v;
        }

        public Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadFileAsync(string source)
        {
            throw new NotImplementedException();
        }
        public Task<string> UploadFileAsync(string fileName, Stream fileStream)
        {
            throw new NotImplementedException();
        }
    }
}
