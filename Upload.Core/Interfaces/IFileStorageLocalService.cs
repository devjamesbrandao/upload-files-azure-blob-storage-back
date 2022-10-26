using Microsoft.AspNetCore.Http;
using Azure.Core.Model;

namespace Azure.Core.Interfaces
{
    public interface IFileStorageLocalService
    {
        Task UploadImageAsync(IFormFile file, string pathToStorageImages);
        Task<IEnumerable<ImageView>> GetAllImagesAsync(string pathToStorageImages);
    }
}