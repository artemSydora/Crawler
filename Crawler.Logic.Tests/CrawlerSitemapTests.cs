using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Crawler.Logic;
using Crawler.Logic.Crawlers.Sitemap;
using static Crawler.Logic.Crawlers.Sitemap.ParserXml;

namespace WebsitePerformanceTool.Tests
{
    public class CrawlerSitemapTests
    {
        private readonly Mock<ContentLoader> _mockContentLoader;
        private readonly Mock<ParserXml> _mockXmlPageParser;
        private readonly Mock<ParserRobots> _mockRobotsParser;
        private readonly CrawlerSitemap _sitemapCrawler;

        public CrawlerSitemapTests()
        {
            _mockContentLoader = new Mock<ContentLoader>(null);
            _mockXmlPageParser = new Mock<ParserXml>();
            _mockRobotsParser = new Mock<ParserRobots>();
            _sitemapCrawler = new CrawlerSitemap(_mockContentLoader.Object, _mockXmlPageParser.Object, _mockRobotsParser.Object);
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrlsAsync_ReturnAllUrlsFromSitemap()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            var fakeUrls = new[]
            {
                "http://www.example.com/About",
                "http://www.example.com/Home"
            };

            var fakeSitemaps = new[]
            {
                "http://www.example.com/sitemap.xml"
            };

            _mockRobotsParser.Setup(rp => rp.ReadRobots(It.IsAny<string>()))
                                            .Returns(fakeSitemaps);
            _mockXmlPageParser.Setup(sp => sp.ParseDocument(It.IsAny<string>(), ParsingOptions.Sitemap))
                                                     .Returns(fakeUrls);
            //act
            var actual = await _sitemapCrawler.GetUrlsAsync(fakeUrl);

            //assert
            Assert.Collection(actual,
                              url => Assert.Equal("http://www.example.com/About", url),
                              url => Assert.Equal("http://www.example.com/Home", url));
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrlsAsync_ShouldParseAllSitemapsInCollection()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            var fakeSitemaps = new[]
            {
                "http://www.example.com/sitemap1.xml",
                "http://www.example.com/sitemap2.xml"
            };


            _mockRobotsParser.Setup(rp => rp.ReadRobots(It.IsAny<string>()))
                                            .Returns(fakeSitemaps);
            _mockXmlPageParser.SetupSequence(sp => sp.ParseDocument(It.IsAny<string>(), ParsingOptions.Sitemap))
                                                     .Returns(Array.Empty<string>())
                                                     .Returns(Array.Empty<string>());
            //act
            var actual = await _sitemapCrawler.GetUrlsAsync(fakeUrl);

            //assert
            _mockXmlPageParser.Verify(sp => sp.ParseDocument(It.IsAny<string>(), ParsingOptions.Sitemap), Times.Exactly(fakeSitemaps.Length));
        }
    }
}
