using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Crawler.Logic.XmlParser;

namespace Crawler.Logic
{
    public class SitemapsCrawler
    {
        private readonly ContentLoader _contentLoader;
        private readonly XmlParser _xmlPageParser;
        private readonly RobotsParser _robotsParser;

        public SitemapsCrawler(ContentLoader sourseLoader, XmlParser sitemapParser, RobotsParser robotsParser)
        {
            _contentLoader = sourseLoader;
            _xmlPageParser = sitemapParser;
            _robotsParser = robotsParser;
        }

        internal virtual async Task<IEnumerable<Uri>> GetUrisAsync(string url)
        {
            var urlsFromSitemaps = new List<Uri>();

            foreach (var sitemapUri in await GetSitemapsUrliAsync(url))
            {
                string content = await _contentLoader.GetContentAsync(sitemapUri.AbsoluteUri);

                IEnumerable<Uri> uris = _xmlPageParser.ParseDocument(content, ParsingOptions.Sitemap);

                urlsFromSitemaps.AddRange(uris);
            }

            return urlsFromSitemaps.ToHashSet();
        }

        private async Task<IEnumerable<Uri>> ParseRobotsAsync(string url)
        {
            var robotsUrl = new Uri(new Uri(url), "/robots.txt").ToString();

            string content = await _contentLoader.GetContentAsync(robotsUrl);

            IEnumerable<Uri> urisFromRobots = _robotsParser.ReadRobots(content);

            return urisFromRobots;
        }

        private async Task<IEnumerable<Uri>> GetSitemapsUrliAsync(string url)
        {
            var sitemaps = new List<Uri>();

            foreach (var uriFromRobots in await ParseRobotsAsync(url))
            {
                string content = await _contentLoader.GetContentAsync(uriFromRobots.AbsoluteUri);

                var sitemapsFromSiteindex = _xmlPageParser.ParseDocument(content, ParsingOptions.Siteindex);

                if (sitemapsFromSiteindex.Count() > 0)
                {
                    sitemaps.AddRange(sitemapsFromSiteindex);
                }
                else
                {
                    sitemaps.Add(uriFromRobots);
                }
            }

            return sitemaps.ToHashSet();
        }
    }
}