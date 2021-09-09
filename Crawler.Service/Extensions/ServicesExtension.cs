using Crawler.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Service.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<TestsService>();
            services.AddScoped<InputValidationService>();

            return services;
        }
    }
}
