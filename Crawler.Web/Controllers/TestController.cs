using System.Threading.Tasks;
using Crawler.Service.Services;
using Crawler.Web.Models;
using Crawler.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly TestsService _testService;
        private readonly InputValidationService _userInputService;
        private readonly Mapper _mapper;

        public TestController(TestsService testService, InputValidationService userInputService, Mapper mapper)
        {
            _testService = testService;
            _userInputService = userInputService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ViewResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var page = await _testService.GetPageAsync(pageNumber, pageSize);

            return View("Tests", _mapper.MapPageViewModel(page));
        }

        [HttpPost]
        public async Task<IActionResult> Index(Input input)
        {
            var isValidUrl = await _userInputService.VerifyUrl(input.Url);

            if (isValidUrl)
            {
                await _testService.SaveTestAsync(input.Url);
            }
            else
            {
                ModelState.AddModelError("Url", _userInputService.ErrorMessage);
            }

            var page = await _testService.GetPageAsync(1, 10);

            return View("Tests", _mapper.MapPageViewModel(page));
        }
    }
}
