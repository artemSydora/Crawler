using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Parser;

namespace Crawler.Logic
{
    public class HtmlParser
    {
        private readonly AngleSharp.Html.Parser.HtmlParser _parser;
        private readonly Verifier _urlVerifier;

        public HtmlParser(Verifier urlVerifier)
        {
            _parser = new AngleSharp.Html.Parser.HtmlParser();
            _urlVerifier = urlVerifier;
        }

        internal virtual IEnumerable<Uri> ParseDocument(string pageUrl, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return Array.Empty<Uri>();
            }

            var document = _parser.ParseDocument(content);

            var pageUri = new Uri(pageUrl);

            var urls = document.QuerySelectorAll("a")
                               .Where(a => a.HasAttribute("href"))
                               .Select(a => a.GetAttribute("href"))
                               .Where(url => !string.IsNullOrEmpty(url))
                               .Where(url => _urlVerifier.VerifyUrl(pageUri, url))
                               .Select(url => new Uri(pageUri, url))
                               .ToHashSet();

            return urls;
        }
    }
}
