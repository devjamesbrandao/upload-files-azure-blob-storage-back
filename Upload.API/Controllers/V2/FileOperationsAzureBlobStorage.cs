using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.API.Controllers.Base;
using Azure.Core.Interfaces;
using Azure.Core.Model;

namespace Azure.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/images")]
    public class FileOperationsAzureBlobStorage : MainController
    {
        private readonly IFileStorageAzureService _azure;

        public FileOperationsAzureBlobStorage(IFileStorageAzureService azure, INotifier notifier) : base(notifier)
        {
            _azure = azure;
        }

        /// <summary>
        /// Get all images from Azure Blob Storage
        /// </summary>
        /// <returns></returns>
        [HttpGet("images")]
        public async Task<ActionResult<List<ImageView>>> GetAllImagesAsync()
        {
            var images = await _azure.GetAllImagesAsync();

            return CustomResponse(images);
        }

        /// <summary>
        /// Upload file to Azure Blob Storage
        /// </summary>
        /// <param name="file">File</param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImageAsync(IFormFile file)
        {
            await _azure.UploadImageAsync(file);

            return CustomResponse();
        }
    }
}