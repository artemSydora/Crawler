using Crawler.Entities.Models;
using Crawler.Logic;
using Crawler.Repository;
using Crawler.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Service.Services
{
    public class TestsService
    {
        private readonly DataAccessor _dataAccess;
        private readonly LinkCollector _linkCollector;
        private readonly PingCollector _pingCollector;

        public TestsService(DataAccessor dataAccess, LinkCollector linkCollector, PingCollector pingCollector)
        {
            _dataAccess = dataAccess;
            _linkCollector = linkCollector;
            _pingCollector = pingCollector;
        }

        public IEnumerable<TestResult> GetAllTests()
        {
            var tests = _dataAccess
                .GetAllTests()
                .Select(tr => new TestResult
                {
                    Id = tr.Id,
                    StartPageUrl = tr.StartPageUrl,
                    DateTime = tr.DateTime
                });

            return tests;
        }

        public async Task<PageModel> GetPageAsync(int pageNumber, int pageSize)
        {
            var page = await _dataAccess
                .GetPageAsync(pageNumber, pageSize);

            var tests = page.Result
                .Select(td => new TestResult
                {
                    Id = td.Id,
                    StartPageUrl = td.StartPageUrl,
                    DateTime = td.DateTime
                })
                .ToList();

            var totalPages = (int)Math.Ceiling(page.TotalCount / (double)pageSize);

            return (new PageModel(pageNumber, totalPages, page.Result));
        }

        public async Task SaveTestResults(string startPageUrl)
        {
            var links = await _linkCollector.CollectAllLinksAsync(startPageUrl);
            var pings = await _pingCollector.MeasureLinksPerformanceAsync(links);

            var measuredLinks = links
                .Select(link => new TestDetail
                {
                    InSitemap = link.InSitemap,
                    InWebsite = link.InWebsite,
                    Url = link.Url,
                    ResponseTimeMs = pings.ToHashSet().SingleOrDefault(ping => ping.Url == link.Url).ResponseTimeMs
                });

            await _dataAccess.SaveTestResultsAsync(startPageUrl, measuredLinks);
        }
    }
}
