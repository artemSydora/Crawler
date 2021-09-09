﻿using Crawler.Entities.Models;
using Crawler.Logic;
using Crawler.Logic.Models;
using Crawler.Repository;
using Crawler.Service.Models;
using Crawler.Service.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Crawler.Service.Tests.Services
{
    public class TestServiceTests
    {
        private readonly Mock<DataAccessor> _mockDataAccessor;
        private readonly TestsService _testService;
        private readonly Mock<LinkCollector> _mockLinkCollector;
        private readonly Mock<PingCollector> _mockPingCollector;

        public TestServiceTests()
        {
            _mockLinkCollector = new Mock<LinkCollector>(null, null);
            _mockPingCollector = new Mock<PingCollector>(null);
            _mockDataAccessor = new Mock<DataAccessor>(null);
            _testService = new TestsService(_mockDataAccessor.Object, _mockLinkCollector.Object, _mockPingCollector.Object);

        }

        [Fact(Timeout = 1000)]
        public async Task GetPageAsync_ShouldReturnPageModel()
        {
            //arrange
            var fakeTests = new List<TestResult>
            {
                new TestResult
                {
                    Id = 1,
                    DateTime = System.DateTime.Today,
                    StartPageUrl = "1",
                    TestDetails = new List<TestDetail>{new TestDetail(), new TestDetail(), new TestDetail() }
                },

                new TestResult
                {
                    Id = 2,
                    DateTime = System.DateTime.Today,
                    StartPageUrl = "2",
                    TestDetails = new List<TestDetail>{ new TestDetail(), new TestDetail(), new TestDetail()}
                }
            };

            var totalTests = 16;
            var pageSize = 5;
            var pageNumber = 3;
            var totalPages = 4;            //(int)Math.Ceiling(totalTests / (double)pageSize);

            var expected = new PageModel(pageNumber, totalPages, fakeTests);

            _mockDataAccessor
                .Setup(da => da.GetPageAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((totalTests, fakeTests));

            //act
            var actual = await _testService.GetPageAsync(pageNumber, pageSize);

            //assert
            Assert.Equal(expected.TotalPages, actual.TotalPages);
            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Collection(actual.Tests,
                testResult => Assert.Equal(fakeTests[0], testResult),
                testResult => Assert.Equal(fakeTests[1], testResult));
        }

        [Fact(Timeout = 1000)]
        public async Task SaveTestResultsAsync_ShouldSaveResultsToDatabase()
        {
            //arrange
            var fakeHomePageUrl = "http://www.example.com";
            var fakeLinks = new[] { new Link() };
            var fakePings = new[] { new Ping() };

            _mockDataAccessor
               .Setup(rda => rda.SaveTestResultsAsync(It.IsAny<string>(), It.IsAny<IEnumerable<TestDetail>>()));

            //act
            await _testService.SaveTestResults(fakeHomePageUrl);

            //assert
            _mockDataAccessor
                .Verify(rda => rda.SaveTestResultsAsync(It.IsAny<string>(), It.IsAny<IEnumerable<TestDetail>>()), Times.Once);
        }

        [Fact(Timeout = 1000)]
        public void GetDetailsByTestId_ReturnDetailsCollection()
        {
            //arrange
            var fakeDetails = new List<TestDetail>
           {
                new TestDetail { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = true },
                new TestDetail { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true },
                new TestDetail { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = false }
            };
            _mockDataAccessor
                .Setup(rda => rda.GetTestById(It.IsAny<int>()))
                .Returns(new TestResult
                {
                    TestDetails = fakeDetails
                });

            //act
            IEnumerable<TestDetail> actual = _testService.GetDetailsByTestId(default);

            //assert
            Assert.Collection(actual,
                result => Assert.Equal(new TestDetail { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = true }, result),
                result => Assert.Equal(new TestDetail { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true }, result),
                result => Assert.Equal(new TestDetail { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = false }, result));
        }
    }
}