using Crawler.Logic.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Crawler.Logic.Tests
{
    public class PingCollectorTests
    {
        private readonly PingCollector _pingCollector;
        private readonly Mock<PingMeter> _mockTimeMeter;

        public PingCollectorTests()
        {
            _mockTimeMeter = new Mock<PingMeter>(null, null);
            _pingCollector = new PingCollector(_mockTimeMeter.Object);
        }

        [Fact]
        public async Task MeasureLinksAsync_ShouldMeasureAllLinks()
        {
            //arrange
            var fakeLinkCollection = new[]
            {
                new Link{ InSitemap = false, InWebsite = false, Url = "http://www.example.com"},
                new Link{ InSitemap = false, InWebsite = false, Url = "http://www.example.com"},
                new Link{ InSitemap = false, InWebsite = false, Url = "http://www.example.com"},
            };

            _mockTimeMeter
                .SetupSequence(tm => tm.Measure(It.IsAny<Link>()))
                .ReturnsAsync(It.IsAny<Ping>())
                .ReturnsAsync(It.IsAny<Ping>())
                .ReturnsAsync(It.IsAny<Ping>());

            //act
            var actual = await _pingCollector.MeasureLinksAsync(fakeLinkCollection);

            //assert
            _mockTimeMeter.Verify(tm => tm.Measure(It.IsAny<Link>()), Times.Exactly(fakeLinkCollection.Length));
        }
    }
}
