using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Parser;
using Crawler.Logic.Website;

namespace Crawler.Logic.Crawlers.Website
{
    public class ParserHtml
    {
        private readonly HtmlParser _parser;
        private readonly Verifier _urlVerifier;

        public ParserHtml(Verifier urlVerifier)
        {
            _parser = new HtmlParser();
            _urlVerifier = urlVerifier;
        }

        internal virtual IEnumerable<string> ParseDocument(string pageUrl, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return Array.Empty<string>();
            }

            var document = _parser.ParseDocument(content);
            
            var pageUri = new Uri(pageUrl);

            var urls = document.QuerySelectorAll("a")
                               .Where(a => a.HasAttribute("href"))
                               .Select(a => a.GetAttribute("href"))
                               .Where(url => !string.IsNullOrEmpty(url))
                               .Where(url => _urlVerifier.VerifyUrl(pageUri, url))
                               .Select(url => new Uri(pageUri, url).ToString())
                               .ToHashSet();

            return urls;
        }
    }
}
