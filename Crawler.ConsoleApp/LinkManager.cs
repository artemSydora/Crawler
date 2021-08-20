using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Logic.Models;
using Crawler.Logic;

namespace Crawler.ConsoleApp
{
    class LinkManager
    {
        private readonly ResponceTimeMeter _responceTimeMeter;

        public LinkManager(ResponceTimeMeter responceTimeMeter)
        {
            _responceTimeMeter = responceTimeMeter;
        }

        public string GetHomePageUrl(string url)
        {
            bool isWellFormedUrl = Uri.TryCreate(url, UriKind.Absolute, out Uri result);

            if (!isWellFormedUrl)
            {
                throw new ArgumentException();
            }

            var homePageUrl = new Uri($"https://{result.Host}").ToString();

            return homePageUrl;
        }

        public IEnumerable<Link> GetUniqueLinksFromSitemap(IEnumerable<Link> links)
        {
            var uniqueLinks = links.Where(l => l.IsFromSitemap && !l.IsFromWebsite);

            return uniqueLinks;
        }

        public IEnumerable<Link> GetUniqueLinksFromWebsite(IEnumerable<Link> links)
        {
            var uniqueLinks = links.Where(l => !l.IsFromSitemap && l.IsFromWebsite);

            return uniqueLinks;
        }

        public async Task<IEnumerable<Ping>> GetPingsSortedByTimingAsync(IEnumerable<Link> links)
        {
            var linksList = new List<Ping>();

            foreach (var link in links)
            {
                Ping ping = await _responceTimeMeter.TimeMeasurement(link);
                
                linksList.Add(ping);
            }

            return linksList.OrderBy(ping => ping.ResponseTime);
        }
    }
}
