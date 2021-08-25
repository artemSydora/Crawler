using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crawler.Logic
{
    public class PingCollector
    {
        private readonly PingMeter _pingMeter;

        public PingCollector(PingMeter pingMeter)
        {
            _pingMeter = pingMeter;
        }

        public async Task<IEnumerable<Ping>> MeasureLinksAsync(IEnumerable<Link> links)
        {
            var pings = new List<Ping>();

            foreach (var link in links)
            {
                var ping = await _pingMeter.Measure(link);

                pings.Add(ping);
            }

            return pings;
        }
    }
}
