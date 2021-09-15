using Crawler.Entities.Models;
using Crawler.Logic;
using Crawler.Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Service.Services
{
    public class TestsService
    {
        private readonly IRepository<TestResult> _testResultsRepository;
        private readonly LinkCollector _linkCollector;
        private readonly PingCollector _pingCollector;

        public TestsService(IRepository<TestResult> testsResultsRepository, LinkCollector linkCollector, PingCollector pingCollector)
        {
            _testResultsRepository = testsResultsRepository;
            _linkCollector = linkCollector;
            _pingCollector = pingCollector;
        }

        public IEnumerable<TestResult> GetAllTests()
        {
            var tests = _testResultsRepository
                .GetAll();

            return tests;
        }

        public async Task<PageModel> GetPageAsync(int pageNumber, int pageSize)
        {
            var tests = _testResultsRepository.Include(t => t.TestDetails);

            var page = await _testResultsRepository
                .GetPageAsync(tests, pageNumber, pageSize);

            var totalPages = (int)Math.Ceiling(page.TotalCount / (double)pageSize);

            return (new PageModel(pageNumber, totalPages, page.Result));
        }

        public async Task SaveTestResultsAsync(string startPageUrl)
        {
            var links = await _linkCollector.CollectAllLinksAsync(startPageUrl);
            var pings = await _pingCollector.MeasureLinksPerformanceAsync(links);

            var details = links
                .Select(link => new TestDetail
                {
                    InSitemap = link.InSitemap,
                    InWebsite = link.InWebsite,
                    Url = link.Url,
                    ResponseTimeMs = pings.SingleOrDefault(ping => ping.Url == link.Url).ResponseTimeMs
                })
                .ToList();

            await _testResultsRepository.AddAsync(new TestResult(startPageUrl, DateTime.Now, details));

            await _testResultsRepository.SaveChangesAsync();
        }

        public IEnumerable<TestDetail> GetDetailsByTestId(int id)
        {
            var testResult = _testResultsRepository
                .Include(t => t.TestDetails)
                .FirstOrDefault(t => t.Id == id);

            return testResult != null ? testResult.TestDetails : Array.Empty<TestDetail>();
        }
    }
}
