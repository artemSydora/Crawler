using System.Diagnostics;
using System.Net.Http;
using System.Xml;
using Crawler.Logic;
using Crawler.Logic.Crawlers.Sitemap;
using Crawler.Logic.Crawlers.Website;
using Crawler.Logic.Website;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CrawlerServiceCollection
    {
        public static IServiceCollection AddCrawlerServices(this IServiceCollection services)
        {
            services.AddScoped<CrawlerWebsite>();
            services.AddScoped<CrawlerSitemap>();
            services.AddScoped<LinkCollector>();
            services.AddScoped<ParserXml>();
            services.AddScoped<ParserHtml>();
            services.AddScoped<Verifier>();
            services.AddSingleton<ContentLoader>();
            services.AddScoped<ResponceTimeMeter>(); ;
            services.AddScoped<Stopwatch>();
            services.AddScoped<XmlDocument>();
            services.AddSingleton<HttpClient>();
            services.AddScoped<ParserRobots>();

            return services;
        }
    }
}
