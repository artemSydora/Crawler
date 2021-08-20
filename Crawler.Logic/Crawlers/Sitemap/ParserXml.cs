using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Crawler.Logic.Crawlers.Sitemap
{
    public class ParserXml
    {
        internal enum ParsingOptions
        {
            Siteindex,
            Sitemap
        }

        internal virtual IEnumerable<string> ParseDocument(string content, ParsingOptions options)
        {
            if (String.IsNullOrEmpty(content))
            {
                return Array.Empty<string>();
            }

            var tagName = String.Empty;

            switch (options)
            {
                case ParsingOptions.Siteindex:
                    tagName = "sitemap";
                    break;

                case ParsingOptions.Sitemap:
                    tagName = "url";
                    break;
            }

            var urls = new List<string>();

            var document = XDocument.Parse(content);

            foreach (var url in document.Root.Elements())
            {
                if (url.Name.LocalName.Contains(tagName))
                {
                    urls.AddRange(ParseElements(url.Elements()));
                }
            }

            return urls;
        }

        private IEnumerable<string> ParseElements(IEnumerable<XElement> elements)
        {
            var urls = new List<string>();

            foreach (var loc in elements)
            {
                if (loc.Name.LocalName.Contains("loc"))
                {
                    urls.Add(loc.Value);
                }
            }

            return urls;
        }
    }
}


