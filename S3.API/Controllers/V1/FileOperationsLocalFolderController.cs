using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S3.API.Controllers.Base;
using S3.Core.Interfaces;

namespace S3.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/files")]
    public class FileOperationsLocalFolderController : MainController
    {
        private readonly IFileStorageLocalService _service;
        
        public FileOperationsLocalFolderController(IFileStorageLocalService service, INotifier notifier) : base(notifier)
        {
            _service = service;
        }

        /// <summary>
        /// Get all files from local folder
        /// </summary>
        /// <returns></returns>
        [HttpGet("files")]
        public async Task<IActionResult> GetAllFilesAsync()
        {
            var images = await _service.GetAllFilesFromLocalFolder(GetCurrentDirectory());

            return CustomResponse(images);
        }

        /// <summary>
        /// Upload file to local folder
        /// </summary>
        /// <param name="file">File</param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            await _service.UploadFileToLocalFolder(file, GetCurrentDirectory());

            return CustomResponse();
        }

        private string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
