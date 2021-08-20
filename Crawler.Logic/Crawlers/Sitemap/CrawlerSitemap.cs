using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Crawler.Logic.Crawlers.Sitemap.ParserXml;

namespace Crawler.Logic.Crawlers.Sitemap
{
    public class CrawlerSitemap
    {
        private readonly ContentLoader _contentLoader;
        private readonly ParserXml _xmlPageParser;
        private readonly ParserRobots _robotsParser;

        public CrawlerSitemap(ContentLoader sourseLoader, ParserXml sitemapParser, ParserRobots robotsParser)
        {
            _contentLoader = sourseLoader;
            _xmlPageParser = sitemapParser;
            _robotsParser = robotsParser;
        }

        internal virtual async Task<IEnumerable<string>> GetUrlsAsync(string url)
        {
            var urlsFromSitemaps = new List<string>();

            foreach (var sitemap in await GetSitemapsAsync(url))
            {
                var content = await _contentLoader.GetContentAsync(sitemap);

                var urls = _xmlPageParser.ParseDocument(content, ParsingOptions.Sitemap);

                urlsFromSitemaps.AddRange(urls);
            }

            return urlsFromSitemaps.ToHashSet();
        }

        private async Task<IEnumerable<string>> ParseRobotsAsync(string url)
        {
            var robotsUrl = new Uri(new Uri(url), "/robots.txt").ToString();

            string content = await _contentLoader.GetContentAsync(robotsUrl);

            IEnumerable<string> urlsFromRobots = _robotsParser.ReadRobots(content);

            return urlsFromRobots;
        }

        private async Task<IEnumerable<string>> GetSitemapsAsync(string url)
        {
            var sitemaps = new List<string>();

            foreach (var urlFromRobots in await ParseRobotsAsync(url))
            {
                var content = await _contentLoader.GetContentAsync(urlFromRobots);

                var sitemapsFromSiteindex = _xmlPageParser.ParseDocument(content, ParsingOptions.Siteindex);

                if (sitemapsFromSiteindex.Count() > 0)
                {
                    sitemaps.AddRange(sitemapsFromSiteindex);
                }
                else
                {
                    sitemaps.Add(urlFromRobots);
                }
            }

            return sitemaps.ToHashSet();
        }
    }
}