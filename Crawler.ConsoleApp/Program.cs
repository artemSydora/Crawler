using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Crawler.Logic;
using Crawler.Repository;

namespace Crawler.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            var consoleApp = host.Services.GetRequiredService<ConsoleApp>();
            
            await consoleApp.Run();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<RepositoryDataAccess>();
                    services.AddEfRepository<CrawlerDbContext>(options =>
                    {
                        options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString"));
                    });

                    services.AddScoped<LinkService>();
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
                })
                .ConfigureLogging(options => options.SetMinimumLevel(LogLevel.Error));
    }
}
