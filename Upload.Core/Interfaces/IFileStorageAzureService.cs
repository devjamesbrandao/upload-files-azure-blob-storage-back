using Azure.Core.Model;
using Microsoft.AspNetCore.Http;

namespace Azure.Core.Interfaces
{
    public interface IFileStorageAzureService
    {
        Task UploadImageAsync(IFormFile file);
        Task<List<ImageView>> GetAllImagesAsync();
    }
}