using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using S3.API.Extensions;
using S3.Core.Interfaces;
using S3.Core.Notifications;
using S3.Infrastructure.CloudStorageServices;
using S3.Infrastructure.LocalStorageServices;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace S3.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<INotifier, Notifier>();

            services.AddScoped<IFileStorageAWSService, FileStorageAWS>();

            services.AddScoped<IFileStorageLocalService, FileStorageLocalService>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}