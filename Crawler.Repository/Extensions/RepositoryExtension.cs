using Crawler.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DataAccessor>();
            services.AddEfRepository<CrawlerDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("WorkDBConnection"));
            });

            return services;
        }
    }
}
