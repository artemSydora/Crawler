using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Crawler.Logic;
using Crawler.Logic.Crawlers.Website;

namespace WebsitePerformanceTool.Tests
{
    public class CrawlerWebsiteTests
    {
        private readonly Mock<ContentLoader> _mockContentLoader;
        private readonly Mock<ParserHtml> _mockHtmlPageParser;
        private readonly CrawlerWebsite _websiteCrawler;

        public CrawlerWebsiteTests()
        {
            _mockContentLoader = new Mock<ContentLoader>(null);
            _mockHtmlPageParser = new Mock<ParserHtml>(null);
            _websiteCrawler = new CrawlerWebsite(_mockContentLoader.Object, _mockHtmlPageParser.Object);
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrlsAsync_AddUniqueUrlsToResult()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            IEnumerable<string> fakeUrls1 = GetFakeUrls1();
            IEnumerable<string> fakeUrls2 = GetFakeUrls2();

            _mockContentLoader.Setup(cl => cl.GetContentAsync(It.IsAny<string>()))
                                             .ReturnsAsync(String.Empty);
            _mockHtmlPageParser.SetupSequence(hpp => hpp.ParseDocument(It.IsAny<string>(), It.IsAny<string>()))
                                                        .Returns(fakeUrls1)
                                                        .Returns(fakeUrls2)
                                                        .Returns(fakeUrls1)
                                                        .Returns(fakeUrls2);

            //act
            var actual = await _websiteCrawler.GetUrlsAsync(fakeUrl);

            //assert
            Assert.Collection(actual,
                              url => Assert.Equal("http://www.example.com/", url),
                              url => Assert.Equal("http://www.example.com/Home", url),
                              url => Assert.Equal("http://www.example.com/About", url),
                              url => Assert.Equal("http://www.example.com/Help", url));
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrlsAsync_ParseContentWhileExistAtLeastOneUniqueUrl()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            IEnumerable<string> fakeUrls1 = GetFakeUrls1();
            IEnumerable<string> fakeUrls2 = GetFakeUrls2();


            var uniqueUrlCount = 4;

            _mockContentLoader.Setup(cl => cl.GetContentAsync(It.IsAny<string>()))
                                             .ReturnsAsync(String.Empty);
            _mockHtmlPageParser.SetupSequence(hpp => hpp.ParseDocument(It.IsAny<string>(), It.IsAny<string>()))
                                                .Returns(fakeUrls1)
                                                .Returns(fakeUrls2)
                                                .Returns(fakeUrls1)
                                                .Returns(fakeUrls2);
            //act
            var actual = await _websiteCrawler.GetUrlsAsync(fakeUrl);

            //assert
            _mockHtmlPageParser.Verify(hpp => hpp.ParseDocument(It.IsAny<string>(), String.Empty), Times.Exactly(uniqueUrlCount));
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrlsAsync_ParseContentAtLeastOnce()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            _mockContentLoader.Setup(cl => cl.GetContentAsync(It.IsAny<string>()))
                                             .ReturnsAsync(String.Empty);
            _mockHtmlPageParser.Setup(hpp => hpp.ParseDocument(It.IsAny<string>(), It.IsAny<string>()))
                                                .Returns(Array.Empty<string>());

            //act
            var actual = await _websiteCrawler.GetUrlsAsync(fakeUrl);

            //assert
            _mockHtmlPageParser.Verify(hpp => hpp.ParseDocument(It.IsAny<string>(), String.Empty), Times.AtLeastOnce);
        }

        #region FakeData

        private IEnumerable<string> GetFakeUrls1()
        {
            var fakeUrls1 = new[]
            {
                "http://www.example.com/Home",
                "http://www.example.com/About"
            };

            return fakeUrls1;
        }

        private IEnumerable<string> GetFakeUrls2()
        {
            var fakeUrls2 = new[]
            {
                "http://www.example.com/Help",
                "http://www.example.com/About"
            };

            return fakeUrls2;
        }

        #endregion
    }
}
