using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Crawler.Logic.Crawlers.Website;
using Crawler.Logic.Crawlers.Sitemap;
using Crawler.Logic;
using Crawler.Logic.Models;

namespace WebsitePerformanceTool.Tests
{
    public class LinksCollectorTests
    {
        private readonly Mock<CrawlerWebsite> _mockWebsiteCrawler;
        private readonly Mock<CrawlerSitemap> _mockSitemapCrawler;
        private readonly LinkCollector _linkCollector;

        public LinksCollectorTests()
        {
            _mockWebsiteCrawler = new Mock<CrawlerWebsite>(null, null);
            _mockSitemapCrawler = new Mock<CrawlerSitemap>(null, null, null);
            _linkCollector = new LinkCollector(_mockWebsiteCrawler.Object, _mockSitemapCrawler.Object);
        }

        [Fact(Timeout = 1000)]
        public async Task CollectAllLinksAsync_ReturnAllUniqueLinksFromWebsiteAndSitemaps()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            IEnumerable<string> fakeUrlsFromSitemap = GetFakeUrlsFromSitemap();
            IEnumerable<string> fakeUrlsFromWebsite = GetFakeUrlsFromWebsite();

            Link[] expected = GetExpectedLinks();

            _mockSitemapCrawler.Setup(url => url.GetUrlsAsync(It.IsAny<string>()))
                                                .ReturnsAsync(fakeUrlsFromSitemap);
            _mockWebsiteCrawler.Setup(url => url.GetUrlsAsync(It.IsAny<string>()))
                                                .ReturnsAsync(fakeUrlsFromWebsite);

            //act
            var actual = await _linkCollector.CollectAllLinksAsync(fakeUrl);

            //assert
            Assert.Collection(actual,
                              link => Assert.Equal(expected[0], link),
                              link => Assert.Equal(expected[1], link),
                              link => Assert.Equal(expected[2], link)
                              );
        }

        [Fact(Timeout = 1000)]
        public async Task CollectAllLinksAsync_IfLinkFromSitemap_ShouldSuportSitemapFlags()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            IEnumerable<string> fakeUrlsFromSitemap = GetFakeUrlsFromSitemap();
            IEnumerable<string> fakeUrlsFromWebsite = GetFakeUrlsFromWebsite();

            IEnumerable<Link> expected = GetExpectedLinks();

            _mockSitemapCrawler.Setup(url => url.GetUrlsAsync(It.IsAny<string>()))
                                                .ReturnsAsync(fakeUrlsFromSitemap);
            _mockWebsiteCrawler.Setup(url => url.GetUrlsAsync(It.IsAny<string>()))
                                                .ReturnsAsync(fakeUrlsFromWebsite);

            //act
            var actual = await _linkCollector.CollectAllLinksAsync(fakeUrl);

            //assert
            Assert.Collection(actual,
                              link => Assert.True(link.IsFromSitemap),
                              link => Assert.True(link.IsFromSitemap),
                              link => Assert.False(link.IsFromSitemap));
        }

        [Fact(Timeout = 1000)]
        public async Task CollectAllLinksAsync_IfLinkFromWebsite_ShouldSupportWebsiteFlags()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            IEnumerable<string> fakeUrlsFromSitemap = GetFakeUrlsFromSitemap();
            IEnumerable<string> fakeUrlsFromWebsite = GetFakeUrlsFromWebsite();

            IEnumerable<Link> expected = GetExpectedLinks();

            _mockSitemapCrawler.Setup(url => url.GetUrlsAsync(It.IsAny<string>()))
                                                .ReturnsAsync(fakeUrlsFromSitemap);
            _mockWebsiteCrawler.Setup(url => url.GetUrlsAsync(It.IsAny<string>()))
                                                .ReturnsAsync(fakeUrlsFromWebsite);

            //act
            var actual = await _linkCollector.CollectAllLinksAsync(fakeUrl);

            //assert

            Assert.Collection(actual,
                              link => Assert.True(link.IsFromWebsite),
                              link => Assert.False(link.IsFromWebsite),
                              link => Assert.True(link.IsFromWebsite));
        }

        #region FakeData

        private Link[] GetExpectedLinks()
        {
            var expected = new[]
            {
                new Link
                {
                    Url = "http://www.example.com/",
                    IsFromSitemap = true,
                    IsFromWebsite = true
                },
                new Link
                {
                    Url = "http://www.example.com/About",
                    IsFromSitemap = true,
                    IsFromWebsite = false
                },
                new Link
                {
                    Url = "http://www.example.com/Home/",
                    IsFromSitemap = false,
                    IsFromWebsite = true
                }
            };

            return expected;
        }

        private IEnumerable<string> GetFakeUrlsFromSitemap()
        {
            var fakeUrlsFromSitemap = new[]
            {
            "http://www.example.com/About",
            "http://www.example.com/"
            };

            return fakeUrlsFromSitemap;
        }

        private IEnumerable<string> GetFakeUrlsFromWebsite()
        {
            var fakeUrlsFromWebsite = new[]
            {
            "http://www.example.com/Home/",
            "http://www.example.com/"
            };

            return fakeUrlsFromWebsite;
        }

        #endregion
    }
}
