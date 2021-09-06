using Crawler.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Web.Controllers
{
    public class DetailController : Controller
    {
        private readonly TestsService _testService;

        public DetailController(TestsService testService)
        {
            _testService = testService;
        }

        public IActionResult Details(int id)
        {
            var details = _testService.GetDetailsByTestId(id);

            return View(details);
        }
    }
}
