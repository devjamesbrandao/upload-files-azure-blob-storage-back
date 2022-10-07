using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace S3.API.Configuration
{
    public static class AWSConfig
    {
        public static IServiceCollection AddAWSConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());

            services.AddAWSService<IAmazonS3>();

            return services;
        }
    }
}