namespace S3.Core.Interfaces
{
    public interface IFileStorageAWSService
    {
        Task CreateBucketAsync(string bucketName);
        Task<IEnumerable<string>> GetAllBucketAsync();
        Task DeleteBucketAsync(string bucketName);
    }
}