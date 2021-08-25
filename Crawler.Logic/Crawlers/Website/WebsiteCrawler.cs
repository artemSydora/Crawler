using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crawler.Logic
{
    public class WebsiteCrawler
    {
        private readonly ContentLoader _contentLoader;
        private readonly HtmlParser _htmlPageParser;

        public WebsiteCrawler(ContentLoader contentLoader, HtmlParser htmlPageParser)
        {
            _contentLoader = contentLoader;
            _htmlPageParser = htmlPageParser;
        }

        internal virtual async Task<IEnumerable<Uri>> GetUrisAsync(string url)
        {
            var newUrls = new Stack<string>();
            newUrls.Push(url);

            var urls = new HashSet<Uri>();
            urls.Add(new Uri(url));

            while (newUrls.Count > 0)
            {
                var pageUrl = newUrls.Pop();

                var content = await _contentLoader.GetContentAsync(pageUrl);

                foreach (var urlFromPage in _htmlPageParser.ParseDocument(pageUrl, content))
                {
                    var isNewPage = urls.Add(urlFromPage);

                    if (isNewPage)
                    {
                        newUrls.Push(urlFromPage.AbsoluteUri);
                    }
                }
            }

            return urls;
        }
    }
}