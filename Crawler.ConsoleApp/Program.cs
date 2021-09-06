using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                    IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                    services.AddRepository(configuration);
                    services.AddLogic();
                    services.AddServices();

                    services.AddScoped<ConsoleApp>();
                    services.AddScoped<Display>();
                    services.AddScoped<ConsoleWrapper>();
                })
                .ConfigureLogging(options => options.SetMinimumLevel(LogLevel.Error));
    }
}
