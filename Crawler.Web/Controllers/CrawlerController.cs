using Crawler.Logic;
using Crawler.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Web.Controllers
{
    public class CrawlerController : Controller
    {
        private readonly LinkCollector _linkCollector;
        private readonly PingCollector _pingCollector;
        private readonly LinkService _linkService;

        public CrawlerController(LinkCollector linkCollector, PingCollector pingCollector, LinkService linkService)
        {
            _linkCollector = linkCollector;
            _pingCollector = pingCollector;
            _linkService = linkService;
        }


        public ViewResult Index()
        {
            var tests = _linkService
                .GetAllTests()
                .Select(t => new Test
                {
                    Id = t.Id,
                    HomePageUrl = t.HomePageUrl,
                    DateTime = t.DateTime
                });

            return View(tests);
        }

        public async Task<IActionResult> Test(string query)
        {
            var links = await _linkCollector.CollectAllLinksAsync(query);
            var pings = await _pingCollector.MeasureLinksAsync(links);
            await _linkService.AddTestResultsAsync(query, links, pings);

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {

            var details = _linkService
                .GetLinksByTestId(id)
                .Select(ml => new TestDetail
                {
                    Url = ml.Url,
                    ResponseTimeMs = ml.ResponseTimeMs
                })
                .OrderBy(d => d.ResponseTimeMs);

            return View("Details", details);
        }

    }
}
