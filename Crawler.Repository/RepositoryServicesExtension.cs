using Crawler.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryServicesExtension
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, string connectionString)
        {
            services.AddEfRepository<CrawlerDbContext>(options =>
            {
                options.ConfigureWarnings(c => c.Log((RelationalEventId.CommandExecuting, LogLevel.None)));
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
