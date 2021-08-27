using System;

namespace Crawler.Logic.Models
{
    public class Link
    {
        public bool InSitemap { get; set; }

        public bool InWebsite { get; set; }

        public string Url { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Link link &&
                   InSitemap == link.InSitemap &&
                   InWebsite == link.InWebsite &&
                   Url == link.Url;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InSitemap, InWebsite, Url);
        }
    }
}
