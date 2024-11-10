using catch_up_backend.Interfaces;

namespace catch_up_backend.FileManagers
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _storagePath;

        public LocalFileStorage(string storagePath)
        {
            this._storagePath = storagePath;
        }

        public Task DeleteFileAsync(string source)
        {
            if (File.Exists(source))
                File.Delete(source);
            return Task.CompletedTask;
        }

        public Task<Stream> DownloadFileAsync(string source)
        {
            if (!File.Exists(source))
            {
                throw new FileNotFoundException("File not found.", source);
            }
            var fileStream = new FileStream(source, FileMode.Open, FileAccess.Read);
            return Task.FromResult<Stream>(fileStream);
        }

        public async Task<string> UploadFileAsync(string uniqFileName, Stream fileStream)
        {
            var path = Path.Combine(_storagePath, uniqFileName);

            using (FileStream fielStreamCreate = File.Create(path))
            {
                await fileStream.CopyToAsync(fielStreamCreate);
            }
            return path;
        }
    }
}
