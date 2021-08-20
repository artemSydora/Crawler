using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Crawler.ConsoleApp
{
    public class Program
    {
        public static async Task Main()
        {
            using var host = Host.CreateDefaultBuilder()
                                 .ConfigureServices((context, services) =>
                                 {
                                     ConfigureServices(services);
                                 })
                                 .Build();

            var crawlerWorker = host.Services.GetRequiredService<LinkWorker>();

            await crawlerWorker.Run();
            await host.RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCrawlerServices();

            services.AddScoped<LinkWorker>();
            services.AddScoped<Display>();
            services.AddScoped<LinkManager>();
            services.AddScoped<ConsoleWrapper>();
        }
    }
}
