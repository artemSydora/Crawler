﻿using Crawler.Logic;
using Crawler.Logic.Crawlers.Sitemap;
using Crawler.Logic.Crawlers.Website;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LogicExtension
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddScoped<WebsiteCrawler>();
            services.AddScoped<SitemapsCrawler>();
            services.AddScoped<LinkCollector>();
            services.AddScoped<XmlDocParser>();
            services.AddScoped<HtmlDocParser>();
            services.AddScoped<RobotsParser>();
            services.AddScoped<Verifier>();
            services.AddSingleton<ContentLoader>();
            services.AddScoped<PingMeter>();
            services.AddScoped<PingCollector>();

            return services;
        }
    }
}
