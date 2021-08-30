using Crawler.Logic;
using Crawler.Logic.Crawlers.Sitemap;
using Crawler.Logic.Crawlers.Website;
using Crawler.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace Crawler.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddScoped<DataAccess>();
            services.AddEfRepository<CrawlerDbContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString"));
            });

            services.AddScoped<LinkService>();
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
            services.AddScoped<Stopwatch>();
            services.AddScoped<XmlDocument>();
            services.AddSingleton<HttpClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Crawler}/{action=Index}/{id?}");
            });
        }
    }
}
