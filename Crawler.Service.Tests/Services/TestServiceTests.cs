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
        private readonly Mock<IRepository<TestDTO>> _mockRepository;
        private readonly TestsService _testService;
        private readonly Mock<LinkCollector> _mockLinkCollector;
        private readonly Mock<PingCollector> _mockPingCollector;

        public TestServiceTests()
        {
            _mockLinkCollector = new Mock<LinkCollector>(null, null);
            _mockPingCollector = new Mock<PingCollector>(null);
            _mockRepository = new Mock<IRepository<TestDTO>>();
            _testService = new TestsService(_mockRepository.Object, _mockLinkCollector.Object, _mockPingCollector.Object);

        }

        [Fact(Timeout = 1000)]
        public void GetAllTestsOrderedById_ShouldReturnTestsCollectionOrderedById()
        {
            //arrange
            IQueryable<TestDTO> fakeTests = new List<TestDTO>
            {
                new TestDTO { Id = 1 },
                new TestDTO { Id = 2 },
                new TestDTO { Id = 3 }
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
        public async Task SaveTestAsync_ShouldSaveResultsToDatabase()
        {
            //arrange
            var fakeHomePageUrl = "http://www.example.com";

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<TestDTO>(), default));
            _mockRepository
               .Setup(r => r.SaveChangesAsync(default));

            //act
            await _testService.SaveTestAsync(fakeHomePageUrl);

            //assert
            _mockRepository
                .Verify(r => r.AddAsync(It.IsAny<TestDTO>(), default), Times.Once);

            _mockRepository
                .Verify(r => r.SaveChangesAsync(default), Times.Once);
        }

        [Fact(Timeout = 1000)]
        public void GetDetailsByTestId_InputExistingId_ReturnDetailsCollection()
        {
            //arrange
            IQueryable<TestDTO> testResult = new List<TestDTO>
            { 
                new TestDTO
                {
                    Id = 1,
                    Details = new List<DetailDTO>
                    {
                    new DetailDTO { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = true },
                    new DetailDTO { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true },
                    new DetailDTO { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = false }
                    }
                } 
            }
            .AsQueryable();

            _mockRepository
                .Setup(r => r.Include(tr => tr.Details))
                .Returns(testResult);

            //act
            IEnumerable<DetailDTO> actual = _testService.GetDetailsByTestId(1);

            //assert
            Assert.Collection(actual,
                result => Assert.Equal(new DetailDTO { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = true }, result),
                result => Assert.Equal(new DetailDTO { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true }, result),
                result => Assert.Equal(new DetailDTO { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = false }, result));
        }

        [Fact(Timeout = 1000)]
        public void GetDetailsByTestId_InputNotExistingId_ReturnEmptyCollection()
        {
            //arrange
            IQueryable<TestDTO> testResult = new List<TestDTO>
            {
                new TestDTO
                {
                    Id = 1,
                    Details = new List<DetailDTO>
                    {
                    new DetailDTO { Url = "1", ResponseTimeMs = 100, InSitemap = true, InWebsite = true },
                    new DetailDTO { Url = "2", ResponseTimeMs = 200, InSitemap = false, InWebsite = true },
                    new DetailDTO { Url = "3", ResponseTimeMs = 300, InSitemap = true, InWebsite = false }
                    }
                }
            }
            .AsQueryable();

            _mockRepository
                .Setup(r => r.Include(tr => tr.Details))
                .Returns(testResult);

            //act
            IEnumerable<DetailDTO> actual = _testService.GetDetailsByTestId(2);

            //assert
            Assert.Empty(actual);
        }
    }
}
