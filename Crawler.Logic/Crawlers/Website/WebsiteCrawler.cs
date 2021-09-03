using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crawler.Logic.Crawlers.Website
{
    public class WebsiteCrawler
    {
        private readonly ContentLoader _contentLoader;
        private readonly HtmlDocParser _htmlDocParser;

        public WebsiteCrawler(ContentLoader contentLoader, HtmlDocParser htmlDocParser)
        {
            _contentLoader = contentLoader;
            _htmlDocParser = htmlDocParser;
        }

        public virtual async Task<IEnumerable<Uri>> GetUrisAsync(string url)
        {
            var newUrls = new Stack<string>();
            newUrls.Push(url);

            var uris = new HashSet<Uri>
            {
                new Uri(url)
            };

            while (newUrls.Count > 0)
            {
                var pageUrl = newUrls.Pop();

                var content = await _contentLoader.GetContentAsync(pageUrl);

                foreach (var urlFromPage in _htmlDocParser.ParseDocument(pageUrl, content))
                {
                    var isNewPage = uris.Add(urlFromPage);

                    if (isNewPage)
                    {
                        newUrls.Push(urlFromPage.AbsoluteUri);
                    }
                }
            }

            return uris;
        }
    }
}