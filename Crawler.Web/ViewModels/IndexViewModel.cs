using Crawler.Web.Models;
using System.Collections.Generic;

namespace Crawler.Web.ViewModels
{
    public class IndexViewModel<T> where T : class
    {
        public IEnumerable<T> Links { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}
