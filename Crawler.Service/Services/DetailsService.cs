using Crawler.Logic.Models;
using Crawler.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Service.Services
{
    public class DetailsService
    {
        private readonly DataAccessor _dataAccess;

        public DetailsService(DataAccessor dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IEnumerable<Ping> GetOrderedPingResultsByTestId(int id)
        {
            var results = _dataAccess
                .GetTestById(id)
                .TestDetails
               .Select(td => new Ping
               {
                   Url = td.Url,
                   ResponseTimeMs = td.ResponseTimeMs
               });

            return results.OrderBy(td => td.ResponseTimeMs);
        }

        public IEnumerable<string> GetUniqueSitemapUrlsByTestId(int id)
        {
            var results = _dataAccess
               .GetTestById(id)
               .TestDetails
               .Where(td => td.InSitemap && !td.InWebsite)
               .Select(td => td.Url);

            return results;
        }

        public IEnumerable<string> GetUniqueWebsiteUrlsByTestId(int id)
        {
            var results = _dataAccess
                .GetTestById(id)
                .TestDetails
                .Where(td => !td.InSitemap && td.InWebsite)
               .Select(td => td.Url);

            return results;
        }

        public (int sitemapCount, int websiteCount) GetUrlCounts(int id)
        {
            var testDetails = _dataAccess
                .GetTestById(id)
                .TestDetails;

            var sitemapCount = testDetails
                .Where(td => td.InSitemap)
                .Count();

            var websiteCount = testDetails
                .Where(td => td.InWebsite)
                .Count();

            return (sitemapCount, websiteCount);
        }
    }
}
