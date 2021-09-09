using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Repository.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEfRepository<CrawlerDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("WorkDBConnection"));
            });

            return services;
        }
    }
}
