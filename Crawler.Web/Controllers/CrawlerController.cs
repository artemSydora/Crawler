using Crawler.Logic;
using Crawler.Web.Models;
using Crawler.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Web.Controllers
{
    public class CrawlerController : Controller
    {
        private readonly LinkCollector _linkCollector;
        private readonly PingCollector _pingCollector;
        private readonly LinkService _linkService;

        private TestDetailViewModel _testDetailViewModel;
        private IEnumerable<Detail> _pings;
        private IEnumerable<string> _sitemapLinks;
        private IEnumerable<string> _websiteLinks;

        public CrawlerController(LinkCollector linkCollector, PingCollector pingCollector, LinkService linkService)
        {
            _linkCollector = linkCollector;
            _pingCollector = pingCollector;
            _linkService = linkService;

            _testDetailViewModel = new TestDetailViewModel();
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

        public IActionResult Details(int id, int page = 1, int pageSize = 10)
        {
            var pings = _linkService
                .GetPingsByTestIdOrderByPing(id)
                .Select(p => new Detail
                {
                    Url = p.Url,
                    ResponseTimeMs = p.ResponseTimeMs
                });

            var sitemapLinks = _linkService
                .GetUniqueSitemapLinksByTestId(id)
                .Select(p => p.Url);

            var websiteLinks = _linkService
                .GetUniqueWebsiteLinksByTestId(id)
                .Select(p => p.Url);

            var pingPage = page;
            var sitemapPage = page;
            var websitePage = page;

            _testDetailViewModel.PingIndex = InitializeIndex<Detail>(pings, pageSize, page);
            _testDetailViewModel.SitemapIndex = InitializeIndex<string>(sitemapLinks, pageSize, page);
            _testDetailViewModel.WebsiteIndex = InitializeIndex<string>(websiteLinks, pageSize, page);

            return View(_testDetailViewModel);
        }

        public IActionResult PingDetails(int page = 1)
        {
            _testDetailViewModel.PingIndex.PageViewModel

            return RedirectToAction("Details", page);
        }

        //public IActionResult SitemapDetails(int pageSize, int pageNumber = 1)
        //{
        //    var count = -_sitemapLinks.Count();

        //    _testDetailViewModel.SitemapIndex = new IndexViewModel<string>
        //    {
        //        PageViewModel = new PageViewModel(count, pageNumber, pageSize),
        //        Links = _sitemapLinks
        //        .Skip((pageNumber - 1) * pageSize).Take(pageSize)
        //    };

        //    return RedirectToAction("Details");
        //}

        //public IActionResult WebsiteDetails(int pageSize, int pageNumber = 1)
        //{
        //    var count = _websiteLinks.Count();

        //    _testDetailViewModel.WebsiteIndex = new IndexViewModel<string>
        //    {
        //        PageViewModel = new PageViewModel(count, pageNumber, pageSize),
        //        Links = _websiteLinks
        //        .Skip((pageNumber - 1) * pageSize).Take(pageSize)
        //    };

        //    return RedirectToAction("Details");
        //}

        private IndexViewModel<T> InitializeIndex<T>(IEnumerable<T> entities, int pageSize, int pageNumber) where T : class
        {
            var count = entities.Count();

            var index = new IndexViewModel<T>
            {
                PageViewModel = new PageViewModel(count, pageNumber, pageSize),
                Links = entities
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
            };

            return index;
        }
    }
}
