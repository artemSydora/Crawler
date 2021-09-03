using Crawler.Service.Services;
using Crawler.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Web.Controllers
{
    public class DetailController : Controller
    {
        private readonly DetailsService _detailsService;

        public DetailController(DetailsService detailsService)
        {
            _detailsService = detailsService;
        }

        public IActionResult Details(int id)
        {
            var counts = _detailsService.GetUrlCounts(id);

            var results = new DetailsViewModel
            {
                PingDetails = _detailsService.GetOrderedPingResultsByTestId(id),
                SitemapDetails = _detailsService.GetUniqueSitemapUrlsByTestId(id),
                WebsiteDetails = _detailsService.GetUniqueWebsiteUrlsByTestId(id),
                SitemapCount = counts.sitemapCount,
                WebsiteCount = counts.websiteCount
            };

            return View(results);
        }
    }
}
