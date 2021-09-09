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
        private readonly IRepository<TestResult> _repository;
        private readonly LinkCollector _linkCollector;
        private readonly PingCollector _pingCollector;

        public TestsService(IRepository<TestResult> repository, LinkCollector linkCollector, PingCollector pingCollector)
        {
            _repository = repository;
            _linkCollector = linkCollector;
            _pingCollector = pingCollector;
        }

        public IEnumerable<TestResult> GetAllTests()
        {
            var tests = _repository
                .GetAll();

            return tests;
        }

        public async Task<PageModel> GetPageAsync(int pageNumber, int pageSize)
        {
            var tests = _repository.Include(t => t.TestDetails);

            var page = await _repository
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
                    ResponseTimeMs = pings.ToHashSet().SingleOrDefault(ping => ping.Url == link.Url).ResponseTimeMs
                })
                .ToList();

            await _repository.AddAsync(new TestResult(startPageUrl, DateTime.Now, details));

            await _repository.SaveChangesAsync();
        }

        public IEnumerable<TestDetail> GetDetailsByTestId(int id)
        {
            var testDetails = _repository
                .Include(t => t.TestDetails)
                .FirstOrDefault(t => t.Id == id)
                .TestDetails;

            return testDetails;
        }
    }
}
