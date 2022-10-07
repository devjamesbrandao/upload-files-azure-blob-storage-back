using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using S3.API.Controllers.Base;
using S3.Core.Interfaces;

namespace S3.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/buckets")]
    public class BucketController : MainController
    {
        private readonly IFileStorageAWSService _s3;

        public BucketController(INotifier notifier, IFileStorageAWSService s3) : base(notifier)
        {
            _s3 = s3;
        }

        /// <summary>
        /// Create Bucket in AWS
        /// </summary>
        /// <param name="bucketName">Bucket's name</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            await _s3.CreateBucketAsync(bucketName);

            return CustomResponse($"Bucket {bucketName} created.");
        }

        /// <summary>
        /// Get all Buckets
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            var buckets = await _s3.GetAllBucketAsync();

            return Ok(buckets);
        }

        /// <summary>
        /// Delete Bucket
        /// </summary>
        /// <param name="bucketName">Bucket's name</param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        {
            await _s3.DeleteBucketAsync(bucketName);

            return NoContent();
        }
    }
}