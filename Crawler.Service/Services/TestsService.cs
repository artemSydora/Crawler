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
        private readonly IRepository<TestDTO> _testResultsRepository;
        private readonly LinkCollector _linkCollector;
        private readonly PingCollector _pingCollector;

        public TestsService(IRepository<TestDTO> testsResultsRepository, LinkCollector linkCollector, PingCollector pingCollector)
        {
            _testResultsRepository = testsResultsRepository;
            _linkCollector = linkCollector;
            _pingCollector = pingCollector;
        }

        public IEnumerable<TestDTO> GetAllTests()
        {
            var tests = _testResultsRepository
                .GetAll();

            return tests;
        }

        public async Task<PageModel> GetPageAsync(int pageNumber, int pageSize)
        {
            var tests = _testResultsRepository.Include(t => t.Details);

            var page = await _testResultsRepository
                .GetPageAsync(tests, pageNumber, pageSize);

            var totalPages = (int)Math.Ceiling(page.TotalCount / (double)pageSize);

            return (new PageModel(pageNumber, totalPages, page.Result));
        }

        public async Task SaveTestAsync(string startPageUrl)
        {
            var links = await _linkCollector.CollectAllLinksAsync(startPageUrl);
            var pings = await _pingCollector.MeasureLinksPerformanceAsync(links);

            var details = links
                .Select(link => new DetailDTO
                {
                    InSitemap = link.InSitemap,
                    InWebsite = link.InWebsite,
                    Url = link.Url,
                    ResponseTimeMs = pings.FirstOrDefault(ping => ping.Url == link.Url).ResponseTimeMs
                })
                .ToList();

            await _testResultsRepository.AddAsync(new TestDTO(startPageUrl, DateTime.Now, details));

            await _testResultsRepository.SaveChangesAsync();
        }

        public IEnumerable<DetailDTO> GetDetailsByTestId(int id)
        {
            var testResult = _testResultsRepository
                .Include(t => t.Details)
                .FirstOrDefault(t => t.Id == id);

            return testResult != null ? testResult.Details : Array.Empty<DetailDTO>();
        }
    }
}
