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

            if (page == null || !page.Tests.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.MapPageViewModel(page));
        }

        /// <summary>
        /// Try save test results
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns>If user input is valid returns true, else returns false and add to model state error message</returns>
        [HttpPost]
        public async Task<IActionResult> RunTest(Input userInput)
        {
            var isValidUrl = await _inputValidationService.VerifyUrl(userInput.Url);

            if (isValidUrl)
            {
                await _testService.RunTestAsync(userInput.Url);

                return Ok(ModelState);
            }
            else
            {
                ModelState.AddModelError("Error", _inputValidationService.ErrorMessage);

                return BadRequest(ModelState);
            }   
        }
    }
}
