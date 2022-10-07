using Amazon.S3;
using S3.Core.Interfaces;
using S3.Core.Notifications;

namespace S3.Infrastructure.CloudStorageServices
{
    public class FileStorageAWS : IFileStorageAWSService
    {
        private readonly IAmazonS3 _s3Client;
        
        private readonly INotifier _notifier;

        public FileStorageAWS(IAmazonS3 s3Client, INotifier notifier)
        {
            _s3Client = s3Client;
            _notifier = notifier;
        }

        public async Task CreateBucketAsync(string bucketName)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

            if(bucketExists)
            {
                _notifier.Handle(new Notification($"Bucket {bucketName} already exists."));
                return;
            }

            await _s3Client.PutBucketAsync(bucketName);
        }

        public async Task DeleteBucketAsync(string bucketName)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);

            if(!bucketExists)
            {
                _notifier.Handle(new Notification($"Bucket {bucketName} doesn't exists."));
                return;
            }

            await _s3Client.DeleteBucketAsync(bucketName);
        }

        public async Task<IEnumerable<string>> GetAllBucketAsync()
        {
            var data = await _s3Client.ListBucketsAsync();

            var buckets = data.Buckets.Select(b => { return b.BucketName; });

            return buckets;
        }
    }
}