namespace Crawler.Logic.Models
{
    public class Ping
    {
        public string Url { get; set; }

        public int ResponseTimeMs { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Ping ping &&
                   Url == ping.Url;
        }

        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }
    }
}
