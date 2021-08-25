using System;

namespace Crawler.Logic
{
    public class Ping
    {
        public string Url { get; set; }

        public int ResponseTime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Ping ping &&
                   Url == ping.Url &&
                   ResponseTime == ping.ResponseTime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Url, ResponseTime);
        }
    }
}
