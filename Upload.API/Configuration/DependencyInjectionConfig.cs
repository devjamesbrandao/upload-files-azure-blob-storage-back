using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Azure.API.Extensions;
using Azure.Core.Interfaces;
using Azure.Core.Notifications;
using Azure.Infrastructure.CloudStorageServices;
using Azure.Infrastructure.LocalStorageServices;
using Swashbuckle.AspNetCore.SwaggerGen;
using Upload.Infrastructure.Utils;

namespace Azure.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INotifier, Notifier>();

            services.Configure<AzureConfig>(configuration.GetSection("AzureConfig"));

            services.AddScoped<IFileStorageAzureService, FileStorageAzure>();

            services.AddScoped<IFileStorageLocalService, FileStorageLocalService>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}