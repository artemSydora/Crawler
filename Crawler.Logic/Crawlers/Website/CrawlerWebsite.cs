using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crawler.Logic.Crawlers.Website
{
    public class CrawlerWebsite
    {
        private readonly ContentLoader _contentLoader;
        private readonly ParserHtml _htmlPageParser;

        public CrawlerWebsite(ContentLoader contentLoader, ParserHtml htmlPageParser)
        {
            _contentLoader = contentLoader;
            _htmlPageParser = htmlPageParser;
        }

        internal virtual async Task<IEnumerable<string>> GetUrlsAsync(string url)
        {
            var newUrls = new Stack<string>();
            newUrls.Push(url);

            var urls = new HashSet<string>();
            urls.Add(url);

            while (newUrls.Count > 0)
            {
                var pageUrl = newUrls.Pop();

                var content = await _contentLoader.GetContentAsync(pageUrl);

                foreach (var urlFromPage in _htmlPageParser.ParseDocument(pageUrl, content))
                {
                    var isNewPage = urls.Add(urlFromPage);

                    if (isNewPage)
                    {
                        newUrls.Push(urlFromPage);
                    }
                }
            }

            return urls;
        }
    }
}