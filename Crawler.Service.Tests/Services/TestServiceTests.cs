using Crawler.Entities.Models;
using Crawler.Logic;
using Crawler.Logic.Models;
using Crawler.Service.Models;
using Crawler.Service.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Crawler.Service.Tests.Services
{
    public class TestServiceTests
    {
        private readonly Mock<IRepository<TestResult>> _mockRepository;
        private readonly TestsService _testService;
        private readonly Mock<LinkCollector> _mockLinkCollector;
        private readonly Mock<PingCollector> _mockPingCollector;

        public TestServiceTests()
        {
            _mockLinkCollector = new Mock<LinkCollector>(null, null);
            _mockPingCollector = new Mock<PingCollector>(null);
            _mockRepository = new Mock<IRepository<TestResult>>();
            _testService = new TestsService(_mockRepository.Object, _mockLinkCollector.Object, _mockPingCollector.Object);

        }

        [Fact(Timeout = 1000)]
        public void GetAllTestsOrderedById_ShouldReturnTestsCollectionOrderedById()
        {
            //arrange
            IQueryable<TestResult> fakeTests = new List<TestResult>
            {
                new TestResult { Id = 1 },
                new TestResult { Id = 2 },
                new TestResult { Id = 3 }
            }
            .AsQueryable();

            _mockRepository
                .Setup(r => r.GetAll())
                .Returns(fakeTests);

            //actual
            var actual = _testService.GetAllTests();

            //assert
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact(Timeout = 1000)]
        public async Task SaveTestResultsAsync_ShouldSaveResultsToDatabase()
        {
            //arrange
            var fakeHomePageUrl = "http://www.example.com";

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<TestResult>(), default));
            _mockRepository
               .Setup(r => r.SaveChangesAsync(default));

            //act
            await _testService.SaveTestResultsAsync(fakeHomePageUrl);

            //assert
            _mockRepository
                .Verify(r => r.AddAsync(It.IsAny<TestResult>(), default), Times.Once);

            _mockRepository
                .Verify(r => r.SaveChangesAsync(default), Times.Once);
        }

        [Fact(Timeout = 1000)]
        public void GetDetailsByTestId_InputExistingId_ReturnDetailsCollection()
        {
            //arrange
            IQueryable<TestResult> testResult = new List<TestResult>
            { 
                new TestResult
                {
                    Id = 1,
                    TestDetails = new List<TestDetail>
                    {
                    new TestDetail { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = true },
                    new TestDetail { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true },
                    new TestDetail { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = false }
                    }
                } 
            }
            .AsQueryable();

            _mockRepository
                .Setup(r => r.Include(tr => tr.TestDetails))
                .Returns(testResult);

            //act
            IEnumerable<TestDetail> actual = _testService.GetDetailsByTestId(1);

            //assert
            Assert.Collection(actual,
                result => Assert.Equal(new TestDetail { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = true }, result),
                result => Assert.Equal(new TestDetail { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true }, result),
                result => Assert.Equal(new TestDetail { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = false }, result));
        }

        [Fact(Timeout = 1000)]
        public void GetDetailsByTestId_InputNotExistingId_ReturnEmptyCollection()
        {
            //arrange
            IQueryable<TestResult> testResult = new List<TestResult>
            {
                new TestResult
                {
                    Id = 1,
                    TestDetails = new List<TestDetail>
                    {
                    new TestDetail { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = true },
                    new TestDetail { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true },
                    new TestDetail { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = false }
                    }
                }
            }
            .AsQueryable();

            _mockRepository
                .Setup(r => r.Include(tr => tr.TestDetails))
                .Returns(testResult);

            //act
            IEnumerable<TestDetail> actual = _testService.GetDetailsByTestId(2);

            //assert
            Assert.Empty(actual);
        }
    }
}
