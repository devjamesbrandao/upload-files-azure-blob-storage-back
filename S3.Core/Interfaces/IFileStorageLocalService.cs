using Microsoft.AspNetCore.Http;
using S3.Core.Model;

namespace S3.Core.Interfaces
{
    public interface IFileStorageLocalService
    {
        Task UploadFileToLocalFolder(IFormFile file, string pathToStorageImages);
        Task<IEnumerable<ImageView>> GetAllFilesFromLocalFolder(string pathToStorageImages);
    }
}