using Crawler.Service.Services;

namespace Microsoft.Extensions.DependencyInjection
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
