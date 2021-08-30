using Crawler.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Web.ViewModels
{
    public class TestDetailViewModel
    {
        public IndexViewModel<Detail> PingIndex { get; set; }

        public IndexViewModel<string> SitemapIndex { get; set; }

        public IndexViewModel<string> WebsiteIndex { get; set; }
    }
}
