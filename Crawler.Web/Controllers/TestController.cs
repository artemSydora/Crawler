using Crawler.Service.Services;
using Crawler.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Crawler.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly TestsService _testService;
        private readonly InputValidationService _userInputService;

        public TestController(TestsService testService, InputValidationService userInputService)
        {
            _testService = testService;
            _userInputService = userInputService;
        }

        [HttpGet]
        public async Task<ViewResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var page = await _testService.GetPageAsync(pageNumber, pageSize);

            return View("Tests", page);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Input input)
        {
            var isValidUrl = await _userInputService.VerifyUlr(input.Url);

            if (isValidUrl)
            {
                await _testService.SaveTestResults(input.Url);
            }
            else
            {
                ModelState.AddModelError("Url", _userInputService.ErrorMessage);
            }

            var page = await _testService.GetPageAsync(1, 10);

            return View("Tests", page);
        }
    }
}
