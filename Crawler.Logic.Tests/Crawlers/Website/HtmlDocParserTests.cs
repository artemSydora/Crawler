using Crawler.Logic.Crawlers.Website;
using Moq;
using System;
using Xunit;

namespace Crawler.Logic.Tests
{
    public class HtmlDocParserTests
    {
        private readonly Mock<Verifier> _mockUrlVerifier;
        private readonly HtmlDocParser _htmlPageParser;

        public HtmlDocParserTests()
        {
            _mockUrlVerifier = new Mock<Verifier>();
            _htmlPageParser = new HtmlDocParser(_mockUrlVerifier.Object);
        }

        [Fact(Timeout = 1000)]
        public void ParseDocument_HtmlContent_SelectNotEmptyHref()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            var fakePage = @"<a href = ""url""></a>
                             <a href = ""url""></a>
                             <a href = ""url""></a>";

            _mockUrlVerifier
                .SetupSequence(un => un.VerifyUri(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(true)
                .Returns(true)
                .Returns(true);

            //act
            var actual = _htmlPageParser.ParseDocument(fakeUrl, fakePage);

            //assert
            _mockUrlVerifier.Verify(un => un.VerifyUri(It.IsAny<Uri>(), It.IsAny<string>()), Times.Exactly(3));
        }

        [Fact(Timeout = 1000)]
        public void ParseDocument_HtmlContent_ShouldIgnoreEmptyHref()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            var fakePage = @"<a href = """"></a>
                             <a href = """"></a>
                             <a href = ""url""></a>";

            _mockUrlVerifier
                .Setup(un => un.VerifyUri(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(true);

            //act
            var actual = _htmlPageParser.ParseDocument(fakeUrl, fakePage);

            //assert
            _mockUrlVerifier.Verify(un => un.VerifyUri(It.IsAny<Uri>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(Timeout = 1000)]
        public void ParseDocument_ContentIsNull_ReturnEmptyCollection()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            //act
            var actualNull = _htmlPageParser.ParseDocument(fakeUrl, null);

            //assert
            Assert.Empty(actualNull);
        }

        [Fact(Timeout = 1000)]
        public void ParseDocument_ContentIsEmpty_ReturnEmptyCollection()
        {
            //arrange
            var fakeUrl = "http://www.example.com";

            //act
            var actualEmpty = _htmlPageParser.ParseDocument(fakeUrl, String.Empty);

            //assert
            Assert.Empty(actualEmpty);
        }
    }
}
