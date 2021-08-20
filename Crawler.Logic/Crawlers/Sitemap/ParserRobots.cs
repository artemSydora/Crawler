using System;
using System.Collections.Generic;

namespace Crawler.Logic.Crawlers.Sitemap
{
    public class ParserRobots
    {
        internal virtual IEnumerable<string> ReadRobots(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return Array.Empty<string>();
            }

            var result = new List<string>();

            var lines = content.Split('\n');

            foreach (string line in lines)
            {
                if (line.Contains("Sitemap:"))
                {
                    foreach (var item in line.Split(" ",StringSplitOptions.RemoveEmptyEntries))
                    {
                        var IsUri = Uri.TryCreate(item, UriKind.Absolute, out Uri sitemapUri)
                            && sitemapUri.HostNameType == UriHostNameType.Dns
                            && sitemapUri.AbsolutePath.Contains(".xml")
                            && !sitemapUri.AbsolutePath.Contains("archive");

                        if(IsUri)
                        {
                            result.Add(sitemapUri.ToString());
                        }
                    }
                }
            }

            return result;
        }
    }
}
