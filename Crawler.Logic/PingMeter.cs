using System.Diagnostics;
using System.Threading.Tasks;

namespace Crawler.Logic
{
    public class PingMeter
    {
        private readonly Stopwatch _timer;
        private readonly ContentLoader _sourseLoader;

        public PingMeter(Stopwatch timer, ContentLoader sourseLoader)
        {
            _timer = timer;
            _sourseLoader = sourseLoader;
        }

        public virtual async Task<Ping> Measure(Link link)
        {
            _timer.Start();

            using var responce = await _sourseLoader.GetResponseAsync(link.Url);

            _timer.Stop();

            var timing = new Ping
            {
                Url = link.Url,
                ResponseTimeMs = (int)_timer.ElapsedMilliseconds
            };

            _timer.Reset();

            return timing;
        }
    }
}
