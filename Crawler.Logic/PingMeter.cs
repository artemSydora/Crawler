using System.Diagnostics;
using System.Threading.Tasks;
using Crawler.Logic.Models;

namespace Crawler.Logic
{
    public class PingMeter
    {
        private readonly Stopwatch _timer;
        private readonly ContentLoader _contentLoader;

        public PingMeter(ContentLoader contentLoader)
        {
            _timer = new Stopwatch();
            _contentLoader = contentLoader;
        }

        public virtual async Task<Ping> Measure(Link link)
        {
            _timer.Start();

            var response = await _contentLoader.GetContentAsync(link.Url);

            _timer.Stop();
            
            var ping = new Ping
            {
                Url = link.Url,
                ResponseTimeMs = (int)_timer.ElapsedMilliseconds
            };

            _timer.Reset();

            return ping;
        }
    }
}
