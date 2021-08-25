using Crawler.Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

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

            services.AddScoped<WebsiteCrawler>();
            services.AddScoped<SitemapsCrawler>();
            services.AddScoped<LinkCollector>();
            services.AddScoped<XmlParser>();
            services.AddScoped<HtmlParser>();
            services.AddScoped<RobotsParser>();
            services.AddScoped<Verifier>();
            services.AddSingleton<ContentLoader>();
            services.AddScoped<PingMeter>(); ;
            services.AddScoped<PingCollector>();
            services.AddScoped<Stopwatch>();
            services.AddScoped<XmlDocument>();
            services.AddSingleton<HttpClient>();

            services.AddScoped<ConsoleApp>();
            services.AddScoped<Display>();
            services.AddScoped<ConsoleWrapper>();
        }
    }
}
