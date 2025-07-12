using Hrms.Core.Abstractions.Services;
using Microsoft.Extensions.Configuration;


namespace Hrms.Infrastructure.Provider
{
    public class StorageService : IStorageService
    {
        private readonly string _storagePath;

        public StorageService(IConfiguration configuration)
        {
            _storagePath = configuration["LocalStorage:Path"];
        }

        public async Task UploadAsync(Stream stream, string fileName, string storagePath)
        {
            string filePath = Path.Combine(storagePath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        public void Delete(string fileName, string storagePath)
        {
            string filePath = Path.Combine(storagePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public Stream Download(string fileName)
        {
            string filePath = Path.Combine(_storagePath, fileName);

            if (File.Exists(filePath))
            {
                return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }

            return null;
        }

    }
}
