using System.Collections.Generic;
using System.Linq;
using Crawler.Entities.Models;
using Crawler.Service.Models;
using Crawler.Web.Models;

namespace Crawler.Web.Services
{
    public class Mapper
    {
        public IEnumerable<DetailViewModel> MapDetailModels(IEnumerable<DetailDTO> details)
        {
            return details
                .Select(detail => new DetailViewModel
                {
                    InSitemap = detail.InSitemap,
                    InWebsite = detail.InWebsite,
                    ResponseTimeMs = detail.ResponseTimeMs,
                    Url = detail.Url
                });
        }

        public IEnumerable<TestViewModel> MapTestViewModel(IEnumerable<TestDTO> tests)
        {
            return tests
                .Select(test => new TestViewModel
                {
                    Id = test.Id,
                    DateTime = test.DateTime,
                    StartPageUrl = test.StartPageUrl
                });
        }

        public PageViewModel MapPageViewModel(PageModel page)
        {
            return new PageViewModel
            {
                CurrentPage = page.CurrentPage,
                Tests = MapTestViewModel(page.Tests).ToList(),
                TotalPages = page.TotalPages
            };
        }
    }
}
