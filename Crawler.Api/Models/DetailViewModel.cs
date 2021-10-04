namespace Crawler.Api.Models
{
    public class DetailViewModel
    {
        public string Url { get; set; }

        public bool InSitemap { get; set; }

        public bool InWebsite { get; set; }

        public int ResponseTimeMs { get; set; }
    }
}
