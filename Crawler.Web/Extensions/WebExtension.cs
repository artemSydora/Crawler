using Crawler.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Web.Extensions
{
    public static class WebExtension
    {
        public static IServiceCollection AddWeb(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            services.AddScoped<Mapper>();

            return services;
        }
    }
}
