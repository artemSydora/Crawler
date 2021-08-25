using Crawler.Entities;
using Moq;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Crawler.Logic.Tests
{
    public class LinkServiceTests
    {
        private readonly Mock<IRepository<Test>> _mockRepository;
        private readonly LinkService _linkService;

        public LinkServiceTests()
        {
            _mockRepository = new Mock<IRepository<Test>>();
            _linkService = new LinkService(_mockRepository.Object);
        }

        [Fact]
        public async Task SaveMeasuredLinks_ShouldAddTestToDatabase()
        {
            //arrange
            var fakeHomePageUrl = "http://www.example.com";
            var fakeLinks = new[] { new Link() };
            var fakePings = new[] { new Ping() };

            //act
            await _linkService.SaveMeasuredLinks(fakeHomePageUrl, fakeLinks, fakePings);

            //assert
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Test>(), default), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(default), Times.Once);
        }
    }
}
