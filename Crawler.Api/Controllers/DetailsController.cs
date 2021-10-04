using System;
using System.Linq;
using Crawler.Api.Models;
using Crawler.Api.Services;
using Crawler.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Api.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}/details")]
    public class DetailsController : ControllerBase
    {
        private readonly TestsService _testService;
        private readonly Mapper _mapper;

        public DetailsController(TestsService testService, Mapper mapper)
        {
            _testService = testService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEquatable<DetailViewModel>> GetTestDetails(int testId)
        {
            var details = _testService.GetDetailsByTestId(testId);

            if (details.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.MapDetailViewModels(details));
        }
    }
}
