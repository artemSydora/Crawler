using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Entities;

namespace Crawler.Logic
{
    public class LinkService
    {
        private readonly IRepository<Test> _repository;

        public LinkService(IRepository<Test> repository)
        {
            _repository = repository;
        }

        public async Task SaveMeasuredLinks(string homePageUrl, IEnumerable<Link> links, IEnumerable<Ping> pings)
        {

            var measuredLinks = links
                .Select(link => new MeasuredLink
                {
                    InSitemap = link.InSitemap,
                    InWebsite = link.InWebsite,
                    Url = link.Url,
                    ResponseTime = pings.SingleOrDefault(ping => ping.Url == link.Url).ResponseTimeMs
                })
                .ToList();

            await _repository.AddAsync(new Test
            {
                HomePageUrl = homePageUrl,
                DateTime = DateTime.Now,
                MeasuredLinks = measuredLinks
            });

            await _repository.SaveChangesAsync();
        }

        public IEnumerable<Ping> GetPingsByHomePageUrlOrderByPing(string homePageUrl)
        {
            var pings = _repository
                .Include(t => t.MeasuredLinks)
                .OrderBy(t => t.Id)
                .LastOrDefault(t => t.HomePageUrl == homePageUrl)
                .MeasuredLinks.Select(ml => new Ping
                {
                    Url = ml.Url,
                    ResponseTimeMs = ml.ResponseTime
                });

            return pings.OrderBy(p => p.ResponseTimeMs);
        }

        public IEnumerable<Link> GetAllLinksByHomePageUrl(string homePageUrl)
        {
            var links = _repository
                .Include(t => t.MeasuredLinks)
                .OrderBy(t => t.Id)
                .LastOrDefault(t => t.HomePageUrl == homePageUrl)
                .MeasuredLinks.Select(ml => LinkFromMeasuredLink(ml));

            return links;
        }

        public IEnumerable<Link> GetLinksFromSitemapByHomePageUrl(string homePageUrl)
        {
            var links = _repository
                .Include(t => t.MeasuredLinks)
                .OrderBy(t => t.Id)
                .LastOrDefault(t => t.HomePageUrl == homePageUrl)
                .MeasuredLinks
                .Where(ml => ml.InSitemap && !ml.InWebsite)
                .Select(ml => LinkFromMeasuredLink(ml));

            return links;
        }

        public IEnumerable<Link> GetLinksFromWebsiteByHomePageUrl(string homePageUrl)
        {
            var links = _repository
                .Include(t => t.MeasuredLinks)
                .OrderBy(t => t.Id)
                .LastOrDefault(t => t.HomePageUrl == homePageUrl)
                .MeasuredLinks
                .Where(l => !l.InSitemap && l.InWebsite)
                .Select(ml => LinkFromMeasuredLink(ml));

            return links;
        }

        private Link LinkFromMeasuredLink(MeasuredLink measuredLink)
        {
            var link = new Link
            {
                InSitemap = measuredLink.InSitemap,
                InWebsite = measuredLink.InWebsite,
                Url = measuredLink.Url,
            };

            return link;
        }
    }
}
