using Crawler.Logic;
using Crawler.Logic.Crawlers.Sitemap;
using Crawler.Logic.Crawlers.Website;
using Crawler.Repository;
using Crawler.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
                    services.AddScoped<DataAccessor>();
                    services.AddEfRepository<CrawlerDbContext>(options =>
                    {
                        options.UseSqlServer("Data Source=DESKTOP-G7JJCOH\\MSSQLSERVER01; Initial Catalog=CrawlerDB; Trusted_Connection=True;");
                    });

                    services.AddScoped<TestsService>();
                    services.AddScoped<DetailsService>();
                    services.AddScoped<InputValidationService>();

                    services.AddScoped<WebsiteCrawler>();
                    services.AddScoped<SitemapsCrawler>();
                    services.AddScoped<LinkCollector>();
                    services.AddScoped<XmlDocParser>();
                    services.AddScoped<HtmlDocParser>();
                    services.AddScoped<RobotsParser>();
                    services.AddScoped<Verifier>();
                    services.AddSingleton<ContentLoader>();
                    services.AddScoped<PingMeter>(); ;
                    services.AddScoped<PingCollector>();

                    services.AddScoped<ConsoleApp>();
                    services.AddScoped<Display>();
                    services.AddScoped<ConsoleWrapper>();
                })
                .ConfigureLogging(options => options.SetMinimumLevel(LogLevel.Error));
    }
}
