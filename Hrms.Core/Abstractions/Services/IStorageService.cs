namespace Hrms.Core.Abstractions.Services
{
    public interface IStorageService
    {
        Task UploadAsync(Stream stream, string fileName, string storagePath);
        void Delete(string fileName, string storagePath);
        Stream Download(string fileName);
    }
}
