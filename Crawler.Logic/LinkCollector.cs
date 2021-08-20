using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Logic.Crawlers.Sitemap;
using Crawler.Logic.Crawlers.Website;
using Crawler.Logic.Models;

namespace Crawler.Logic
{
    public class LinkCollector
    {
        private readonly CrawlerWebsite _websiteCrawler;
        private readonly CrawlerSitemap _sitemapCrawler;

        public LinkCollector(CrawlerWebsite websiteCrawler, CrawlerSitemap sitemapCrawler)
        {
            _websiteCrawler = websiteCrawler;
            _sitemapCrawler = sitemapCrawler;
        }

        public virtual async Task<IEnumerable<Link>> CollectAllLinksAsync(string homePageUrl)
        {
            IEnumerable<string> urlsFromSitemap = await _sitemapCrawler.GetUrlsAsync(homePageUrl);
            IEnumerable<string> urlsFromWebsite = await _websiteCrawler.GetUrlsAsync(homePageUrl);

            var allLinks = MergeLinks(urlsFromWebsite, urlsFromSitemap);

            return allLinks;
        }

        private IEnumerable<Link> MergeLinks(IEnumerable<string> urlsFromWebsite, IEnumerable<string> urlsFromSitemap)
        {
            List<Link> allLinks = urlsFromSitemap.Intersect(urlsFromWebsite)
                                                .Select(s => new Link
                                                {
                                                    IsFromSitemap = true,
                                                    IsFromWebsite = true,
                                                    Url = s
                                                })
                                                .ToList();

            allLinks.AddRange(urlsFromSitemap.Except(urlsFromWebsite)
                                             .Select(s => new Link
                                             {
                                                 IsFromSitemap = true,
                                                 IsFromWebsite = false,
                                                 Url = s
                                             }));

            allLinks.AddRange(urlsFromWebsite.Except(urlsFromSitemap)
                                             .Select(s => new Link
                                             {
                                                 IsFromSitemap = false,
                                                 IsFromWebsite = true,
                                                 Url = s
                                             }));

            return allLinks;
        }
    }
}
