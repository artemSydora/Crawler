namespace Crawler.Entities.Models
{
    public class MeasuredLink
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool InSitemap { get; set; }

        public bool InWebsite { get; set; }

        public int ResponseTimeMs { get; set; }
    }
}
