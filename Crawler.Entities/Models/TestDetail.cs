using System;

namespace Crawler.Entities.Models
{
    public class TestDetail
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool InSitemap { get; set; }

        public bool InWebsite { get; set; }

        public int ResponseTimeMs { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TestDetail detail &&
                   Id == detail.Id &&
                   Url == detail.Url &&
                   InSitemap == detail.InSitemap &&
                   InWebsite == detail.InWebsite &&
                   ResponseTimeMs == detail.ResponseTimeMs;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Url, InSitemap, InWebsite, ResponseTimeMs);
        }
    }
}
