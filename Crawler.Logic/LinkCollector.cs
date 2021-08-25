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
            List<Link> allLinks = urlsFromSitemap
                .Intersect(urlsFromWebsite)
                .Select(uri => new Link
                {
                    IsFromSitemap = true,
                    IsFromWebsite = true,
                    Url = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}"
                })
                .ToList();

            allLinks.AddRange(urlsFromSitemap
                .Except(urlsFromWebsite)
                .Select(uri => new Link
                {
                    IsFromSitemap = true,
                    IsFromWebsite = false,
                    Url = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}"
                }));

            allLinks.AddRange(urlsFromWebsite
                .Except(urlsFromSitemap)
                .Select(uri => new Link
                {
                    IsFromSitemap = false,
                    IsFromWebsite = true,
                    Url = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}"
                }));

            return allLinks;
        }
    }
}
