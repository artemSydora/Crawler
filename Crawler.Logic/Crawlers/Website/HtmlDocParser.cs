using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Logic.Crawlers.Website
{
    public class HtmlDocParser
    {
        private readonly HtmlParser _parser;
        private readonly Verifier _verifier;

        public HtmlDocParser(Verifier verifier)
        {
            _parser = new HtmlParser();
            _verifier = verifier;
        }

        public virtual IEnumerable<Uri> ParseDocument(string pageUrl, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return Array.Empty<Uri>();
            }

            var document = _parser.ParseDocument(content);

            var pageUri = new Uri(pageUrl);

            var urls = document
                .QuerySelectorAll("a")
                .Where(a => a.HasAttribute("href"))
                .Select(a => a.GetAttribute("href"))
                .Where(url => !string.IsNullOrEmpty(url))
                .Where(url => _verifier.VerifyUri(pageUri, url))
                .Select(url => new Uri(pageUri, url))
                .ToHashSet();

            return urls;
        }
    }
}
