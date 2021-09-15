using Crawler.Logic.Crawlers.Sitemap;
using Crawler.Logic.Crawlers.Website;
using Crawler.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Logic
{
    public class LinkCollector
    {
        private readonly WebsiteCrawler _websiteCrawler;
        private readonly SitemapsCrawler _sitemapCrawler;

        public LinkCollector(WebsiteCrawler websiteCrawler, SitemapsCrawler sitemapCrawler)
        {
            _websiteCrawler = websiteCrawler;
            _sitemapCrawler = sitemapCrawler;
        }

        public virtual async Task<IEnumerable<Link>> CollectAllLinksAsync(string homePageUrl)
        {
            IEnumerable<Uri> urlsFromSitemap = await _sitemapCrawler.GetUrisAsync(homePageUrl);
            IEnumerable<Uri> urlsFromWebsite = await _websiteCrawler.GetUrisAsync(homePageUrl);

            var allLinks = MergeLinks(urlsFromWebsite, urlsFromSitemap, new CustomUriComparer());

            return allLinks;
        }

        private IEnumerable<Link> MergeLinks(IEnumerable<Uri> urlsFromWebsite, IEnumerable<Uri> urlsFromSitemap, IEqualityComparer<Uri> comparer)
        {
            var allLinks = urlsFromSitemap
                .Union(urlsFromWebsite, comparer)
                .Select(uri => new Link
                {
                    Url = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}",
                    InSitemap = urlsFromSitemap.Contains(uri, comparer),
                    InWebsite = urlsFromWebsite.Contains(uri, comparer)
                })
                .ToHashSet();

            return allLinks;
        }
    }
}
