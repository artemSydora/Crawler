using System;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Api.Models;
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
        private readonly InputValidationService _inputValidationService;
        private readonly Mapper _mapper;

        public TestController(TestsService testService, InputValidationService inputValidationService, Mapper mapper)
        {
            _testService = testService ?? throw new ArgumentNullException(nameof(testService));
            _inputValidationService = inputValidationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns tests page
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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


        [HttpPost]
        public async Task<IActionResult> SaveResults(Input userInput)
        {
            var isValidUrl = await _inputValidationService.VerifyUrl(userInput.Url);

            if (isValidUrl)
            {
                await _testService.SaveTestAsync(userInput.Url);
                var page = await _testService.GetPageAsync(1, 10);

                return Ok(_mapper.MapPageViewModel(page));
            }
            else
            {
                ModelState.AddModelError("Url", _inputValidationService.ErrorMessage);

                return BadRequest(ModelState);
            }

            
        }
    }
}
