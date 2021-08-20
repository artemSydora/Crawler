using System;

namespace Crawler.Logic.Models
{
    public class Link
    {
        public bool IsFromSitemap { get; set; }

        public bool IsFromWebsite { get; set; }

        public string Url { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Link link &&
                   IsFromSitemap == link.IsFromSitemap &&
                   IsFromWebsite == link.IsFromWebsite &&
                   Url == link.Url;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsFromSitemap, IsFromWebsite, Url);
        }
    }
}
