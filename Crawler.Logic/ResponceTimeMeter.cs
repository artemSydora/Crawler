using System.Diagnostics;
using System.Threading.Tasks;
using Crawler.Logic.Models;

namespace Crawler.Logic
{
    public class ResponceTimeMeter
    {
        private readonly Stopwatch _timer;
        private readonly ContentLoader _sourseLoader;

        public ResponceTimeMeter(Stopwatch timer, ContentLoader sourseLoader)
        {
            _timer = timer;
            _sourseLoader = sourseLoader;
        }

        public async Task<Ping> TimeMeasurement(Link link)
        {
            _timer.Start();

            using var responce = await _sourseLoader.GetResponseAsync(link.Url);

            _timer.Stop();

            var timing = new Ping
                        {
                            Url = link.Url,
                            ResponseTime = (int)_timer.ElapsedMilliseconds
                        };

            _timer.Reset();

            return timing;
        }
    }
}
