using Crawler.Entities.Models;
using System.Collections.Generic;

namespace Crawler.Service.Models
{
    public class PageModel
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<TestResult> Tests { get; set; }

        public PageModel(int currentPage, int totalPages, IList<TestResult> result)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            Tests = result;
        }
    }
}
