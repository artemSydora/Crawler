using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Entities;
using Crawler.Repository;

namespace Crawler.Logic
{
    public class LinkService
    {
        private readonly RepositoryDataAccess _repositoryDataAccess;

        public LinkService(RepositoryDataAccess repositoryDataAccess)
        {
            _repositoryDataAccess = repositoryDataAccess;
        }

        public async Task AddTestResultsAsync(string homePageUrl, IEnumerable<Link> links, IEnumerable<Ping> pings)
        {

            var measuredLinks = links
                .Select(link => new MeasuredLink
                {
                    InSitemap = link.InSitemap,
                    InWebsite = link.InWebsite,
                    Url = link.Url,
                    ResponseTimeMs = pings.SingleOrDefault(ping => ping.Url == link.Url).ResponseTimeMs
                })
                .ToList();

            await _repositoryDataAccess.SaveTestResultAsync(homePageUrl, measuredLinks);
        }

        public IEnumerable<Ping> GetPingsByUrlOrderByPing(string homePageUrl)
        {
            var pings = _repositoryDataAccess
                .GetTestsByHomePageUrl(homePageUrl)
                .MeasuredLinks
                .Select(ml => new Ping
                {
                    Url = ml.Url,
                    ResponseTimeMs = ml.ResponseTimeMs
                });

            return pings.OrderBy(p => p.ResponseTimeMs);
        }

        public IEnumerable<Link> GetUniqueSitemapLinksByUrl(string homePageUrl)
        {
            var links = _repositoryDataAccess
                .GetTestsByHomePageUrl(homePageUrl)
                .MeasuredLinks
                .Where(ml => ml.InSitemap && !ml.InWebsite)
                .Select(ml => LinkFromMeasuredLink(ml));

            return links;
        }

        public IEnumerable<Link> GetUniqueWebsiteLinksByUrl(string homePageUrl)
        {
            var links = _repositoryDataAccess
                .GetTestsByHomePageUrl(homePageUrl)
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
