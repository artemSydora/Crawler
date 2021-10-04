using System.Collections.Generic;
using System.Linq;
using Crawler.Entities.Models;
using Crawler.Service.Models;
using Crawler.Api.Models;

namespace Crawler.Api.Services
{
    public class Mapper
    {
        public IEnumerable<DetailViewModel> MapDetailViewModels(IEnumerable<DetailDTO> details)
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

        public IEnumerable<TestViewModel> MapTestViewModels(IEnumerable<TestDTO> tests)
        {
            return tests
                .Select(test => new TestViewModel
                {
                    DateTime = test.DateTime,
                    StartPageUrl = test.StartPageUrl
                });
        }

        public PageViewModel MapPageViewModel(PageModel page)
        {
            return new PageViewModel
            {
                CurrentPage = page.CurrentPage,
                Tests = MapTestViewModels(page.Tests).ToList(),
                TotalPages = page.TotalPages
            };
        }
    }
}
