using Crawler.Logic.Crawlers.Sitemap;
using Crawler.Logic.Crawlers.Website;
using Crawler.Logic.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Crawler.Logic.Tests
{
    public class LinksCollectorTests
    {
        private readonly Mock<WebsiteCrawler> _mockWebsiteCrawler;
        private readonly Mock<SitemapsCrawler> _mockSitemapCrawler;
        private readonly LinkCollector _linkCollector;

        public LinksCollectorTests()
        {
            _mockWebsiteCrawler = new Mock<WebsiteCrawler>(null, null);
            _mockSitemapCrawler = new Mock<SitemapsCrawler>(null, null, null);
            _linkCollector = new LinkCollector(_mockWebsiteCrawler.Object, _mockSitemapCrawler.Object);
        }

        [Fact(Timeout = 1000)]
        public async Task CollectAllLinksAsync_ReturnAllUniqueLinksFromWebsiteAndSitemaps()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            IEnumerable<Uri> fakeUrlsFromSitemap = GetFakeUrlsFromSitemap();
            IEnumerable<Uri> fakeUrlsFromWebsite = GetFakeUrlsFromWebsite();

            Link[] expected = GetExpectedLinks();

            _mockSitemapCrawler
                .Setup(url => url.GetUrisAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUrlsFromSitemap);
            _mockWebsiteCrawler
                .Setup(url => url.GetUrisAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUrlsFromWebsite);

            //act
            var actual = await _linkCollector.CollectAllLinksAsync(fakeUrl);

            //assert
            Assert.Collection(actual,
                link => Assert.Equal(expected[0], link),
                link => Assert.Equal(expected[1], link),
                link => Assert.Equal(expected[2], link));
        }

        [Fact(Timeout = 1000)]
        public async Task CollectAllLinksAsync_IfLinkFromSitemap_ShouldSuportSitemapFlags()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            IEnumerable<Uri> fakeUrlsFromSitemap = GetFakeUrlsFromSitemap();
            IEnumerable<Uri> fakeUrlsFromWebsite = GetFakeUrlsFromWebsite();

            IEnumerable<Link> expected = GetExpectedLinks();

            _mockSitemapCrawler
                .Setup(url => url.GetUrisAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUrlsFromSitemap);
            _mockWebsiteCrawler
                .Setup(url => url.GetUrisAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUrlsFromWebsite);

            //act
            var actual = await _linkCollector.CollectAllLinksAsync(fakeUrl);

            //assert
            Assert.Collection(actual,
                link => Assert.True(link.InSitemap),
                link => Assert.True(link.InSitemap),
                link => Assert.False(link.InSitemap));
        }

        [Fact(Timeout = 1000)]
        public async Task CollectAllLinksAsync_IfLinkFromWebsite_ShouldSupportWebsiteFlags()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            IEnumerable<Uri> fakeUrlsFromSitemap = GetFakeUrlsFromSitemap();
            IEnumerable<Uri> fakeUrlsFromWebsite = GetFakeUrlsFromWebsite();

            IEnumerable<Link> expected = GetExpectedLinks();

            _mockSitemapCrawler
                .Setup(url => url.GetUrisAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUrlsFromSitemap);
            _mockWebsiteCrawler
                .Setup(url => url.GetUrisAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeUrlsFromWebsite);

            //act
            var actual = await _linkCollector.CollectAllLinksAsync(fakeUrl);

            //assert

            Assert.Collection(actual,
                link => Assert.True(link.InWebsite),
                link => Assert.False(link.InWebsite),
                link => Assert.True(link.InWebsite));
        }

        #region FakeData

        private Link[] GetExpectedLinks()
        {
            var expected = new[]
            {
                new Link
                {
                    Url = "http://www.example.com/",
                    InSitemap = true,
                    InWebsite = true
                },
                new Link
                {
                    Url = "http://www.example.com/About",
                    InSitemap = true,
                    InWebsite = false
                },
                new Link
                {
                    Url = "https://www.example.com/Home/",
                    InSitemap = false,
                    InWebsite = true
                }
            };

            return expected;
        }

        private IEnumerable<Uri> GetFakeUrlsFromSitemap()
        {
            var fakeUrisFromSitemap = new[]
            {
                new Uri("http://www.example.com/"),
                new Uri("http://www.example.com/About")
            };

            return fakeUrisFromSitemap;
        }

        private IEnumerable<Uri> GetFakeUrlsFromWebsite()
        {
            var fakeUrisFromWebsite = new[]
            {
                new Uri("https://www.example.com/"),
                new Uri("https://www.example.com/Home/")
            };

            return fakeUrisFromWebsite;
        }

        #endregion
    }
}
