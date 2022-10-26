using Microsoft.AspNetCore.Http;
using Azure.Core.Interfaces;
using Azure.Core.Model;
using Azure.Core.Notifications;

namespace Azure.Infrastructure.LocalStorageServices
{
    public class FileStorageLocalService : IFileStorageLocalService
    {
        private readonly INotifier _notifier;

        public FileStorageLocalService(INotifier notifier)
        {
            _notifier = notifier;
        }

        private string CombinedPath(string pathToStorageImages)
        {
            return Path.Combine(pathToStorageImages, "wwwroot/images");
        }

        public async Task<IEnumerable<ImageView>> GetAllImagesAsync(string pathToStorageImages)
        {
            try
            {
                string path = CombinedPath(pathToStorageImages);

                var images = Directory.EnumerateFiles(path).Select(x => new ImageView
                {
                    Url = $"http://localhost:5000/images/{Path.GetFileName(x)}",
                    Name = Path.GetFileName(x)
                });

                return await Task.FromResult(images);
            }
            catch(Exception ex)
            {
                _notifier.Handle(new Notification("Error: " + ex.Message));
                return null;
            }
        }

        public async Task UploadImageAsync(IFormFile file, string pathToStorageImages)
        {
            try
            {
                string path = CombinedPath(pathToStorageImages);

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                FileInfo fileInfo = new FileInfo(file.FileName);

                string fileNameWithPath = Path.Combine(path, file.FileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch(Exception ex)
            {
                _notifier.Handle(new Notification("Error: " + ex.Message));
            }
        }
    }
}