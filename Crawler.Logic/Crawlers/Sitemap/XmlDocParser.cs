using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Crawler.Logic.Crawlers.Sitemap
{
    public class XmlDocParser
    {
        public enum ParsingOptions
        {
            Siteindex,
            Sitemap
        }

        public virtual IEnumerable<Uri> ParseDocument(string content, ParsingOptions options)
        {
            if (String.IsNullOrEmpty(content))
            {
                return Array.Empty<Uri>();
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

            var urls = new List<Uri>();

            var document = XDocument.Parse(content);

            foreach (var url in document.Root.Elements())
            {
                if (url.Name.LocalName.Contains(tagName))
                {
                    IEnumerable<Uri> elements = ParseElements(url.Elements());
                    
                    urls.AddRange(elements);
                }
            }

            return urls;
        }

        private IEnumerable<Uri> ParseElements(IEnumerable<XElement> elements)
        {
            var urls = new List<Uri>();

            foreach (var loc in elements)
            {
                if (loc.Name.LocalName.Contains("loc"))
                {
                    urls.Add(new Uri(loc.Value));
                }
            }

            return urls;
        }
    }
}


