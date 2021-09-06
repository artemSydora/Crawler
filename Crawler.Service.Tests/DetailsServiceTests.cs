using Crawler.Entities.Models;
using Crawler.Logic.Models;
using Crawler.Repository;
using Crawler.Service.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Crawler.Service.Tests
{
    public class DetailsServiceTests
    {
        private readonly Mock<DataAccessor> _mockDataAccessor;
        private readonly DetailsService _detailService;

        public DetailsServiceTests()
        {
            _mockDataAccessor = new Mock<DataAccessor>(null);
            _detailService = new DetailsService(_mockDataAccessor.Object);
        }


        [Fact]
        public void GetPingResultsByTestId_ReturnPingsCollection()
        {
            //arrange
            var fakeMeaseredLinkCollection = GetFakeMeasuredLinks();

            _mockDataAccessor
                .Setup(rda => rda.GetTestById(It.IsAny<int>()))
                .Returns(new TestResult
                {
                    TestDetails = fakeMeaseredLinkCollection
                });

            //act
            IEnumerable<Ping> actual = _detailService.GetOrderedPingResultsByTestId(default);

            //assert
            Assert.Collection(actual,
                result => Assert.Equal(new Ping { Url = "1", ResponseTimeMs = 100 }, result),
                result => Assert.Equal(new Ping { Url = "2", ResponseTimeMs = 200 }, result),
                result => Assert.Equal(new Ping { Url = "3", ResponseTimeMs = 300 }, result));
        }



        [Fact]
        public void GetSitemapResultsByTestId_ReturnLinksCollection()
        {
            //arrange
            var fakeMeaseredLinkCollection = GetFakeMeasuredLinks();

            _mockDataAccessor
                .Setup(rda => rda.GetTestById(It.IsAny<int>()))
                .Returns(new TestResult
                {
                    TestDetails = fakeMeaseredLinkCollection
                });

            //act
            IEnumerable<string> actual = _detailService.GetUniqueSitemapUrlsByTestId(default);

            //assert
            Assert.Collection(actual,
                link => Assert.Equal("1", link));
        }

        [Fact]
        public void GetWebsiteResultsByTestId_ReturnLinksCollection()
        {
            //arrange
            var fakeMeaseredLinkCollection = GetFakeMeasuredLinks();

            _mockDataAccessor
                .Setup(rda => rda.GetTestById(It.IsAny<int>()))
                .Returns(new TestResult
                {
                    TestDetails = fakeMeaseredLinkCollection
                });

            //act
            IEnumerable<string> actual = _detailService.GetUniqueWebsiteUrlsByTestId(default);

            //assert
            Assert.Collection(actual,
                link => Assert.Equal("2", link));
        }

        [Fact]
        public void Get_ReturnPingsCollection()
        {
            //arrange
            var fakeMeaseredLinkCollection = GetFakeMeasuredLinks();

            _mockDataAccessor
                .Setup(rda => rda.GetTestById(It.IsAny<int>()))
                .Returns(new TestResult
                {
                    TestDetails = fakeMeaseredLinkCollection
                });

            //act
            (int sitemapCount, int websiteCount) actual = _detailService.GetUrlCounts(default);

            //assert
            Assert.Equal(2, actual.sitemapCount);
            Assert.Equal(2, actual.websiteCount);
        }

        #region FakeData

        private List<TestDetail> GetFakeMeasuredLinks()
        {
            var fakeMeaseredLinkCollection = new List<TestDetail>
           {
                new TestDetail { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true },
                new TestDetail { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = true },
                new TestDetail { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = false }
           };

            return fakeMeaseredLinkCollection;
        }

        #endregion
    }
}
