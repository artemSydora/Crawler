using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Crawler.Logic;

namespace Crawler.ConsoleApp
{
    public class ConsoleApp
    {
        private readonly Display _display;
        private readonly LinkCollector _linkCollector;
        private readonly PingCollector _pingCollector;
        private readonly LinkService _linkService;
        private readonly ConsoleWrapper _consoleWrapper;

        public ConsoleApp(Display display, LinkCollector linkCollector,PingCollector pingCollector, LinkService linkService, ConsoleWrapper consoleWrapper)
        {
            _display = display;
            _linkCollector = linkCollector;
            _pingCollector = pingCollector;
            _linkService = linkService;
            _consoleWrapper = consoleWrapper;
        }

        public async Task Run()
        {
            _consoleWrapper.WtiteLine("Please, input website URL or press <Enter> to exit...");
            var input = _consoleWrapper.ReadLine();

            while (!String.IsNullOrEmpty(input))
            {
                try
                {
                    var homePageUrl = GetHomePageUrl(input);

                    IEnumerable<Link> links = await _linkCollector.CollectAllLinksAsync(homePageUrl);
                    IEnumerable<Ping> pings = await _pingCollector.MeasureLinksAsync(links);

                    await _linkService.SaveMeasuredLinks(homePageUrl, links, pings);
                    
                    IEnumerable<Link> uniqueUrlsFromSitemap = _linkService.GetLinksFromSitemapByHomePageUrl(homePageUrl);
                    IEnumerable<Link> uniqueUrlsFromWebsite = _linkService.GetLinksFromWebsiteByHomePageUrl(homePageUrl);

                    _display.ShowTable("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site", uniqueUrlsFromSitemap, "URL");
                    _display.ShowTable("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml", uniqueUrlsFromWebsite, "URL");

                    IEnumerable<Ping> pingsInDb = _linkService.GetPingsByHomePageUrlOrderByPing(homePageUrl);

                    _display.ShowTable("Timing", pingsInDb, "URL", "Timing");

                    var websiteUrlCount = links.Where(b => b.InWebsite).Count();
                    var sitemapUrlCount = links.Where(b => b.InSitemap).Count();

                    _consoleWrapper.WtiteLine($"Urls(html documents) found after crawling a website: {websiteUrlCount}");
                    _consoleWrapper.WtiteLine($"Urls found in sitemap: {sitemapUrlCount}");
                }
                catch (ArgumentException ex)
                {
                    _consoleWrapper.WtiteLine(ex.Message);
                }
                catch(HttpRequestException ex)
                {
                    _consoleWrapper.WtiteLine(ex.Message);
                }

                _consoleWrapper.WtiteLine("Please, input website URL or press <Enter> to exit...");

                input = _consoleWrapper.ReadLine();
            }
        }

        public string GetHomePageUrl(string url)
        {
            bool isWellFormedUrl = Uri.TryCreate(url, UriKind.Absolute, out Uri result);

            if (!isWellFormedUrl)
            {
                throw new ArgumentException();
            }

            var homePageUrl = new Uri($"https://{result.Host}").ToString();

            return homePageUrl;
        }
    }
}
