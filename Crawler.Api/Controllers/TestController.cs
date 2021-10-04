using System;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Api.Services;
using Crawler.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Api.Controllers
{
    [ApiController]
    [Route("api/tests")]
    public class TestController : ControllerBase
    {
        private readonly TestsService _testService;
        private readonly Mapper _mapper;

        public TestController(TestsService testService, Mapper mapper)
        {
            _testService = testService ?? throw new ArgumentNullException(nameof(testService));
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTestsPage(int pageNumber, int pageSize)
        {
            var page = await _testService.GetPageAsync(pageNumber, pageSize);

            if (page == null || page.Tests.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.MapPageViewModel(page));
        }
    }
}
