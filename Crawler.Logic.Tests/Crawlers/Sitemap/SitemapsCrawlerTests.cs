using Crawler.Logic.Crawlers.Sitemap;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using static Crawler.Logic.Crawlers.Sitemap.XmlDocParser;

namespace Crawler.Logic.Tests
{
    public class SitemapsCrawlerTests
    {
        private readonly Mock<ContentLoader> _mockContentLoader;
        private readonly Mock<XmlDocParser> _mockXmlPageParser;
        private readonly Mock<RobotsParser> _mockRobotsParser;
        private readonly SitemapsCrawler _sitemapCrawler;

        public SitemapsCrawlerTests()
        {
            _mockContentLoader = new Mock<ContentLoader>(null);
            _mockXmlPageParser = new Mock<XmlDocParser>();
            _mockRobotsParser = new Mock<RobotsParser>();
            _sitemapCrawler = new SitemapsCrawler(_mockContentLoader.Object, _mockXmlPageParser.Object, _mockRobotsParser.Object);
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrliAsync_ReturnAllUrisFromSitemap()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            var fakeUrls = new[]
            {
                new Uri("http://www.example.com/About"),
                new Uri("http://www.example.com/Home")
            };

            var fakeSitemaps = new[]
            {
                new Uri("http://www.example.com/sitemap.xml")
            };

            _mockRobotsParser
                .Setup(rp => rp.ReadRobots(It.IsAny<string>()))
                .Returns(fakeSitemaps);
            _mockXmlPageParser
                .Setup(sp => sp.ParseDocument(It.IsAny<string>(), ParsingOptions.Sitemap))
                .Returns(fakeUrls);

            //act
            var actual = await _sitemapCrawler.GetUrisAsync(fakeUrl);

            //assert
            Assert.Collection(actual,
                url => Assert.Equal(new Uri("http://www.example.com/About"), url),
                url => Assert.Equal(new Uri("http://www.example.com/Home"), url));
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrisAsync_ShouldParseAllSitemapsInCollection()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            var fakeSitemaps = new[]
            {
                new Uri("http://www.example.com/sitemap1.xml"),
                new Uri("http://www.example.com/sitemap2.xml")
            };

            _mockRobotsParser
                .Setup(rp => rp.ReadRobots(It.IsAny<string>()))
                .Returns(fakeSitemaps);
            _mockXmlPageParser
                .SetupSequence(sp => sp.ParseDocument(It.IsAny<string>(), ParsingOptions.Sitemap))
                .Returns(Array.Empty<Uri>())
                .Returns(Array.Empty<Uri>());

            //act
            var actual = await _sitemapCrawler.GetUrisAsync(fakeUrl);

            //assert
            _mockXmlPageParser.Verify(sp => sp.ParseDocument(It.IsAny<string>(), ParsingOptions.Sitemap), Times.Exactly(fakeSitemaps.Length));
        }
    }
}
