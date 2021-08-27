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
            List<Link> allLinks = urlsFromSitemap
                .Intersect(urlsFromWebsite, comparer)
                .Select(uri => new Link
                {
                    InSitemap = true,
                    InWebsite = true,
                    Url = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}"
                })
                .ToList();

            allLinks
                .AddRange(urlsFromSitemap
                .Except(urlsFromWebsite, comparer)
                .Select(uri => new Link
                {
                    InSitemap = true,
                    InWebsite = false,
                    Url = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}"
                }));

            allLinks
                .AddRange(urlsFromWebsite
                .Except(urlsFromSitemap, comparer)
                .Select(uri => new Link
                {
                    InSitemap = false,
                    InWebsite = true,
                    Url = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}"
                }));

            return allLinks;
        }
    }
}
