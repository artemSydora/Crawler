using Crawler.Logic;
using Crawler.Logic.Crawlers.Sitemap;
using Crawler.Logic.Crawlers.Website;
using Crawler.Repository;
using Crawler.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddScoped<DataAccessor>();
            services.AddEfRepository<CrawlerDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("HomeDBConnection"));
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //env.EnvironmentName = "Production";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Test}/{action=Index}/{id?}");
            });
        }
    }
}
