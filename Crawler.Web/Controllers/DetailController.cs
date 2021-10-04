using Crawler.Service.Services;
using Crawler.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Web.Controllers
{
    public class DetailController : Controller
    {
        private readonly TestsService _testService;
        private readonly Mapper _mapper;

        public DetailController(TestsService testService, Mapper mapper)
        {
            _testService = testService;
            _mapper = mapper;
        }

        public IActionResult Details(int id)
        {
            var details = _testService.GetDetailsByTestId(id);

            return View(_mapper.MapDetailViewModels(details));
        }
    }
}
