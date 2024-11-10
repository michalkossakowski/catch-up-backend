using catch_up_backend.Interfaces;

namespace catch_up_backend.FileManagers
{
    public class FileStorageFactory
    {
        private readonly IConfiguration _configuration;
        public FileStorageFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IFileStorage CreateFileStorage()
        {
            string storageType = _configuration["StorageType"] ?? throw new InvalidOperationException("Type of file storage string 'StorageType' not found.");
            return storageType switch
            {
                "Local" => new LocalFileStorage(_configuration["LocalStoragePath"]
                    ?? throw new InvalidOperationException("LocalStoragePath configuration not found.")),

                "Azure" => new AzureBlobStorage(
                    _configuration["AzureBlobStorage:ConnectionString"]
                        ?? throw new InvalidOperationException("Azure Blob Storage connection string not found."),
                    _configuration["AzureBlobStorage:ContainerName"]
                        ?? throw new InvalidOperationException("Azure Blob Storage container name not found.")
                ),

                _ => throw new NotSupportedException($"Unsupported storage type: {storageType}")
            };
        }
    }
}
