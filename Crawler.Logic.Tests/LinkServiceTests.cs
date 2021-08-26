using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Crawler.Entities;
using Crawler.Repository;

namespace Crawler.Logic.Tests
{
    public class LinkServiceTests
    {
        private readonly Mock<RepositoryDataAccess> _mockRepositoryDataAccess;
        private readonly LinkService _linkService;

        public LinkServiceTests()
        {
            _mockRepositoryDataAccess = new Mock<RepositoryDataAccess>(null);
            _linkService = new LinkService(_mockRepositoryDataAccess.Object);
        }

        [Fact]
        public async Task AddTestResultsAsync_ShouldPassResultsToDatabase()
        {
            //arrange
            var fakeHomePageUrl = "http://www.example.com";
            var fakeLinks = new[] { new Link() };
            var fakePings = new[] { new Ping() };

            _mockRepositoryDataAccess
               .Setup(rda => rda.SaveTestResultAsync(It.IsAny<string>(), It.IsAny<IEnumerable<MeasuredLink>>()));

            //act
            await _linkService.AddTestResultsAsync(fakeHomePageUrl, fakeLinks, fakePings);

            //assert
            _mockRepositoryDataAccess
                .Verify(rda => rda.SaveTestResultAsync(It.IsAny<string>(), It.IsAny<IEnumerable<MeasuredLink>>()), Times.Once);
        }

        [Fact]
        public void GetPingsByUrlOrderByPing_ReturnOrderedPingsCollection()
        {
            //arrange
            var fakeMeaseredLinkCollection = GetFakeMeasuredLinks();

            _mockRepositoryDataAccess
                .Setup(rda => rda.GetTestsByHomePageUrl(It.IsAny<string>()))
                .Returns(new Test
                {
                    MeasuredLinks = fakeMeaseredLinkCollection
                });

            //act
            IEnumerable<Ping> actual = _linkService.GetPingsByUrlOrderByPing(String.Empty);

            //assert
            Assert.Collection(actual,
                ping => Assert.Equal(new Ping { Url = "1", ResponseTimeMs = 100 }, ping),
                ping => Assert.Equal(new Ping { Url = "2", ResponseTimeMs = 200 }, ping),
                ping => Assert.Equal(new Ping { Url = "3", ResponseTimeMs = 300 }, ping));
        }

        [Fact]
        public void GetUniqueWebsiteLinksByUrl_ReturnLinksCollection()
        {
            //arrange
            var fakeMeaseredLinkCollection = GetFakeMeasuredLinks();

            _mockRepositoryDataAccess
                .Setup(rda => rda.GetTestsByHomePageUrl(It.IsAny<string>()))
                .Returns(new Test
                {
                    MeasuredLinks = fakeMeaseredLinkCollection
                });

            //act
            IEnumerable<Link> actual = _linkService.GetUniqueWebsiteLinksByUrl(String.Empty);

            //assert
            Assert.Collection(actual,
                link => Assert.Equal(new Link { Url = "2", InSitemap = false, InWebsite = true }, link));
        }

        [Fact]
        public void GetUniqueSitemapLinksByUrl_ReturnLinksCollection()
        {
            //arrange
            var fakeMeaseredLinkCollection = GetFakeMeasuredLinks();

            _mockRepositoryDataAccess
                .Setup(rda => rda.GetTestsByHomePageUrl(It.IsAny<string>()))
                .Returns(new Test
                {
                    MeasuredLinks = fakeMeaseredLinkCollection
                });

            //act
            IEnumerable<Link> actual = _linkService.GetUniqueSitemapLinksByUrl(String.Empty);

            //assert
            Assert.Collection(actual,
                link => Assert.Equal(new Link { Url = "1", InSitemap = true, InWebsite = false }, link));
        }

        #region FakeData

        private List<MeasuredLink> GetFakeMeasuredLinks()
        {
            var fakeMeaseredLinkCollection = new List<MeasuredLink>
           {
                new MeasuredLink { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true },
                new MeasuredLink { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = true },
                new MeasuredLink { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = false }
           };

            return fakeMeaseredLinkCollection;
        }

        #endregion
    }
}
