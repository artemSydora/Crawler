using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Logic;
using Crawler.Logic.Models;

namespace Crawler.ConsoleApp
{
    class LinkWorker
    {
        private readonly Display _display;
        private readonly LinkCollector _linkCollector;
        private readonly LinkManager _linkManager;
        private readonly ConsoleWrapper _consoleWrapper;

        public LinkWorker(Display display, LinkCollector linkCollector, LinkManager linkManager, ConsoleWrapper consoleWrapper)
        {
            _display = display;
            _linkCollector = linkCollector;
            _linkManager = linkManager;
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
                    var homePageUrl = _linkManager.GetHomePageUrl(input);

                    IEnumerable<Link> links = await _linkCollector.CollectAllLinksAsync(homePageUrl);

                    IEnumerable<Link> uniqueUrlsFromSitemap = _linkManager.GetUniqueLinksFromSitemap(links);
                    IEnumerable<Link> uniqueUrlsFromWebsite = _linkManager.GetUniqueLinksFromWebsite(links);

                    _display.ShowTable("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site", uniqueUrlsFromSitemap, "URL");
                    _display.ShowTable("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml", uniqueUrlsFromWebsite, "URL");

                    IEnumerable<Ping> timing = await _linkManager.GetPingsSortedByTimingAsync(links);

                    _display.ShowTable("Timing", timing, "URL", "Timing");

                    var websiteUrlCount = links.Where(b => b.IsFromWebsite).Count();
                    var sitemapUrlCount = links.Where(b => b.IsFromSitemap).Count();

                    _consoleWrapper.WtiteLine($"Urls(html documents) found after crawling a website: {websiteUrlCount}");
                    _consoleWrapper.WtiteLine($"Urls found in sitemap: {sitemapUrlCount}");
                }
                catch (ArgumentException ex)
                {
                    _consoleWrapper.WtiteLine(ex.Message);
                }

                _consoleWrapper.WtiteLine("Please, input website URL or press <Enter> to exit...");

                input = _consoleWrapper.ReadLine();
            }
        }
    }
}
