using Crawler.Logic.Models;
using System.Collections.Generic;

namespace Crawler.Web.ViewModels
{
    public class DetailsViewModel
    {
        public int SitemapCount { get; set; }

        public int WebsiteCount { get; set; }

        public IEnumerable<Ping> PingDetails { get; set; }

        public IEnumerable<string> SitemapDetails { get; set; }

        public IEnumerable<string> WebsiteDetails { get; set; }
    }
}
