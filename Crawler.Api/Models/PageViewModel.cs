using System.Collections.Generic;

namespace Crawler.Api.Models
{
    public class PageViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<TestViewModel> Tests { get; set; }
    }
}
