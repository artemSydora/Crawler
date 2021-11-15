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
        private readonly IRepository<TestDTO> _testsRepository;
        private readonly LinkCollector _linkCollector;
        private readonly PingCollector _pingCollector;

        public TestsService(IRepository<TestDTO> testsRepository, LinkCollector linkCollector, PingCollector pingCollector)
        {
            _testsRepository = testsRepository;
            _linkCollector = linkCollector;
            _pingCollector = pingCollector;
        }

        public IEnumerable<TestDTO> GetAllTests()
        {
            var tests = _testsRepository.GetAll();

            return tests;
        }

        public async Task<PageModel> GetPageAsync(int pageNumber, int pageSize)
        {
            var tests = _testsRepository.GetAll();

            var page = await _testsRepository
                .GetPageAsync(tests, pageNumber, pageSize);

            var totalPages = (int)Math.Ceiling(page.TotalCount / (double)pageSize);

            return (new PageModel(pageNumber, totalPages, page.Result));
        }

        public async Task RunTestAsync(string startPageUrl)
        {
            var startPageUri = new Uri(startPageUrl);
            var testingUrl = startPageUri.Scheme + "://" + startPageUri.Host;

            var links = await _linkCollector.CollectAllLinksAsync(testingUrl);
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

            await _testsRepository.AddAsync(new TestDTO(testingUrl, DateTime.Now, details));

            await _testsRepository.SaveChangesAsync();
        }

        public IEnumerable<DetailDTO> GetDetailsByTestId(int id)
        {
            var testResult = _testsRepository
                .Include(t => t.Details)
                .FirstOrDefault(t => t.Id == id);

            return testResult != null ? testResult.Details : Array.Empty<DetailDTO>();
        }
    }
}
