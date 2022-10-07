using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S3.API.Controllers.Base;
using S3.Core.Interfaces;
using S3.Core.Model;

namespace S3.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/files")]
    public class FileOperationsAWSS3 : MainController
    {
        private readonly IAmazonS3 _s3Client;

        public FileOperationsAWSS3(IAmazonS3 s3Client, INotifier notifier) : base(notifier)
        {
            _s3Client = s3Client;
        }

        /// <summary>
        /// Get all files from AWS S3
        /// </summary>
        /// <returns></returns>
        [HttpGet("files")]
        public async Task<IActionResult> GetAllFilesAsync(string bucketName, string prefix)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");

            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = prefix
            };

            var result = await _s3Client.ListObjectsV2Async(request);
            
            var s3Objects = result.S3Objects.Select(s =>
                {
                    var urlRequest = new GetPreSignedUrlRequest()
                    {
                        BucketName = bucketName,
                        Key = s.Key,
                        Expires = DateTime.UtcNow.AddMinutes(1)
                    };

                    return new ImageView()
                    {
                        Name = s.Key.ToString(),
                        Url = _s3Client.GetPreSignedURL(urlRequest),
                    };
                }
            );

            return Ok(s3Objects);
        }

        /// <summary>
        /// Upload file to AWS S3
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="bucketName">Bucket's name</param>
        /// <param name="prefix">Prefix of file</param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file, string bucketName, string prefix)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };

            request.Metadata.Add("Content-Type", file.ContentType);

            await _s3Client.PutObjectAsync(request);

            return Ok($"File {prefix}/{file.FileName} uploaded to S3 successfully!");
        }
    }
}