using System.Text.RegularExpressions;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Azure.Core.Interfaces;
using Azure.Core.Model;
using Azure.Core.Notifications;
using Upload.Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace Azure.Infrastructure.CloudStorageServices
{
    public class FileStorageAzure : IFileStorageAzureService
    {
        private readonly INotifier _notifier;

        private readonly BlobContainerClient _blob;

        public FileStorageAzure(INotifier notifier, IOptions<AzureConfig> config)
        {
            _notifier = notifier;
            _blob = new BlobContainerClient(config.Value.ConnectionString, config.Value.ContainerName);
        }

        public async Task UploadImageAsync(IFormFile file)
        {
            try
            {
                var fileName = Guid.NewGuid().ToString() + ".jpg";
                
                string base64Image = "";
                
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);

                    var fileBytes = ms.ToArray();

                    base64Image = Convert.ToBase64String(fileBytes);
                }

                var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, ""); 
                
                byte[] imageBytes = Convert.FromBase64String(data);

                var blob = _blob.GetBlobClient(fileName);

                using(var stream = new MemoryStream(imageBytes))
                {
                    await blob.UploadAsync(stream);
                }
            }
            catch(Exception)
            {
                _notifier.Handle(new Notification("Erro ao realizar upload da imagem"));
            }
        }

        public async Task<List<ImageView>> GetAllImagesAsync()
        {
            var images = new List<ImageView>();

            try
            {
                await foreach (var blob in _blob.GetBlobsAsync())
                {
                    var image = _blob.GetBlobClient(blob.Name);

                    images.Add(new ImageView(image.Name, image.Uri.ToString()));
                }

                return images;
            }
            catch (RequestFailedException)
            {
                _notifier.Handle(new Notification("Erro ao buscar imagens"));
                return null;
            }
        }
    }
}