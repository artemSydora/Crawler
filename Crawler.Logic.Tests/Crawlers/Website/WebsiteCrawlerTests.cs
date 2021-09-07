using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Crawler.Logic.Crawlers.Website;

namespace Crawler.Logic.Tests.Crawlers.Website
{
    public class WebsiteCrawlerTests
    {
        private readonly Mock<ContentLoader> _mockContentLoader;
        private readonly Mock<HtmlDocParser> _mockHtmlPageParser;
        private readonly WebsiteCrawler _websiteCrawler;

        public WebsiteCrawlerTests()
        {
            _mockContentLoader = new Mock<ContentLoader>();
            _mockHtmlPageParser = new Mock<HtmlDocParser>(null);
            _websiteCrawler = new WebsiteCrawler(_mockContentLoader.Object, _mockHtmlPageParser.Object);
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrisAsync_AddUniqueUrisToResult()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            IEnumerable<Uri> fakeUris1 = GetFakeUrls1();
            IEnumerable<Uri> fakeUris2 = GetFakeUrls2();

            _mockContentLoader
                .Setup(cl => cl.GetContentAsync(It.IsAny<string>()))
                .ReturnsAsync(String.Empty);
            _mockHtmlPageParser
                .SetupSequence(hpp => hpp.ParseDocument(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeUris1)
                .Returns(fakeUris2)
                .Returns(fakeUris1)
                .Returns(fakeUris2);

            //act
            var actual = await _websiteCrawler.GetUrisAsync(fakeUrl);

            //assert
            Assert.Collection(actual,
                uri => Assert.Equal(new Uri("http://www.example.com/"), uri),
                uri => Assert.Equal(new Uri("http://www.example.com/Home"), uri),
                uri => Assert.Equal(new Uri("http://www.example.com/About"), uri),
                uri => Assert.Equal(new Uri("http://www.example.com/Help"), uri));
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrisAsync_ParseContentWhileExistAtLeastOneUniqueUri()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            IEnumerable<Uri> fakeUris1 = GetFakeUrls1();
            IEnumerable<Uri> fakeUris2 = GetFakeUrls2();


            var uniqueUrlCount = 4;

            _mockContentLoader
                .Setup(cl => cl.GetContentAsync(It.IsAny<string>()))
                .ReturnsAsync(String.Empty);
            _mockHtmlPageParser
                .SetupSequence(hpp => hpp.ParseDocument(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeUris1)
                .Returns(fakeUris2)
                .Returns(fakeUris1)
                .Returns(fakeUris2);
            //act
            var actual = await _websiteCrawler.GetUrisAsync(fakeUrl);

            //assert
            _mockHtmlPageParser.Verify(hpp => hpp.ParseDocument(It.IsAny<string>(), String.Empty), Times.Exactly(uniqueUrlCount));
        }

        [Fact(Timeout = 1000)]
        public async Task GetUrisAsync_ParseContentAtLeastOnce()
        {
            //arrange
            var fakeUrl = "http://www.example.com/";

            _mockContentLoader
                .Setup(cl => cl.GetContentAsync(It.IsAny<string>()))
                .ReturnsAsync(String.Empty);
            _mockHtmlPageParser
                .Setup(hpp => hpp.ParseDocument(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Array.Empty<Uri>());

            //act
            var actual = await _websiteCrawler.GetUrisAsync(fakeUrl);

            //assert
            _mockHtmlPageParser.Verify(hpp => hpp.ParseDocument(It.IsAny<string>(), String.Empty), Times.AtLeastOnce);
        }

        #region FakeData

        private IEnumerable<Uri> GetFakeUrls1()
        {
            var fakeUrls1 = new[]
            {
                new Uri("http://www.example.com/Home"),
                new Uri("http://www.example.com/About")
            };

            return fakeUrls1;
        }

        private IEnumerable<Uri> GetFakeUrls2()
        {
            var fakeUrls2 = new[]
            {
                new Uri("http://www.example.com/Help"),
                new Uri("http://www.example.com/About")
            };

            return fakeUrls2;
        }

        #endregion
    }
}
