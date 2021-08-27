using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Crawler.Logic.Crawlers.Sitemap.XmlDocParser;

namespace Crawler.Logic.Crawlers.Sitemap
{
    public class SitemapsCrawler
    {
        private readonly ContentLoader _contentLoader;
        private readonly XmlDocParser _xmlPageParser;
        private readonly RobotsParser _robotsParser;

        public SitemapsCrawler(ContentLoader sourseLoader, XmlDocParser sitemapParser, RobotsParser robotsParser)
        {
            _contentLoader = sourseLoader;
            _xmlPageParser = sitemapParser;
            _robotsParser = robotsParser;
        }

        public virtual async Task<IEnumerable<Uri>> GetUrisAsync(string url)
        {
            var urisFromSitemaps = new List<Uri>();

            foreach (var sitemapUri in await GetSitemapsUrisAsync(url))
            {
                string content = await _contentLoader.GetContentAsync(sitemapUri.AbsoluteUri);

                IEnumerable<Uri> uris = _xmlPageParser.ParseDocument(content, ParsingOptions.Sitemap);

                urisFromSitemaps.AddRange(uris);
            }

            return urisFromSitemaps.ToHashSet();
        }

        private async Task<IEnumerable<Uri>> ParseRobotsAsync(string url)
        {
            var robotsUrl = new Uri(new Uri(url), "/robots.txt").ToString();

            string content = await _contentLoader.GetContentAsync(robotsUrl);

            IEnumerable<Uri> urisFromRobots = _robotsParser.ReadRobots(content);

            return urisFromRobots;
        }

        private async Task<IEnumerable<Uri>> GetSitemapsUrisAsync(string url)
        {
            var sitemapsUris = new List<Uri>();

            foreach (var uriFromRobots in await ParseRobotsAsync(url))
            {
                string content = await _contentLoader.GetContentAsync(uriFromRobots.AbsoluteUri);

                var sitemapsFromSiteindex = _xmlPageParser.ParseDocument(content, ParsingOptions.Siteindex);

                if (sitemapsFromSiteindex.Count() > 0)
                {
                    sitemapsUris.AddRange(sitemapsFromSiteindex);
                }
                else
                {
                    sitemapsUris.Add(uriFromRobots);
                }
            }

            return sitemapsUris.ToHashSet();
        }
    }
}