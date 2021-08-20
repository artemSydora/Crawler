using System;
using Xunit;
using Crawler.Logic.Website;

namespace WebsitePerformanceTool.Tests
{
    public class VerifierTests
    {
        private readonly Verifier _urlNormalizer;

        public VerifierTests()
        {
            _urlNormalizer = new Verifier();
        }

        [Theory]
        [InlineData("https://www.contoso.com/Home/", "https://www.contoso.com/About/")]
        [InlineData("https://www.contoso.com/Home/", "https://www.contoso.com/About/Index.htm")]
        [InlineData("https://www.contoso.com/Home/", "https://www.contoso.com/About/Index.html")]
        public void NormalizeUrl_PathIsAbsoluteUr_VerifyUrlsEnding(string baseUrl, string path)
        {
            //arrange
            var baseUri = new Uri(baseUrl);

            //act
            var actual = _urlNormalizer.VerifyUrl(baseUri, path);

            //assert
            Assert.True(actual);
        }

        [Fact]
        public void NormalizeUrl_ShouldVerifyEqualsDomains()
        {
            //arrange
            var baseUri = new Uri("https://www.contoso.com/Home/");
            var path = "https://www.contoso.com/About/Index.htm";

            //act
            var actual = _urlNormalizer.VerifyUrl(baseUri, path);

            //assert
            Assert.True(actual);
        }

        [Fact]
        public void VerifyUrl_ShouldVerifyDifferenceDomains()
        {
            //arrange
            var baseUri = new Uri("https://www.contoso.com/Home/");
            var path = "https://www.google.com/Home/Index.htm";

            //act
            var actual = _urlNormalizer.VerifyUrl(baseUri, path);

            //assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("https://www.contoso.com/Home/", "About/")]
        [InlineData("https://www.contoso.com/Home/", "About/Index.htm")]
        [InlineData("https://www.contoso.com/Home/", "About/Index.html")]
        public void NormalizeUrl_PathIsRelativeUrl_VerifyUrlsEnding(string baseUrl, string path)
        {
            //arrange
            var baseUri = new Uri(baseUrl);

            //act
            var actual = _urlNormalizer.VerifyUrl(baseUri, path);

            //assert
            Assert.True(actual);
        }

        [Fact]
        public void NormalizeUrl_PathContainsFragment_ReturnFalse()
        {
            //arrange
            var baseUri = new Uri("https://www.contoso.com");
            var path = "/Home/Index.htm#FragmentName";

            //act
            var actual = _urlNormalizer.VerifyUrl(baseUri, path);

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void NormalizeUrl_PathContainsQuery_ReturnFalse()
        {
            //arrange
            var baseUri = new Uri("https://www.contoso.com");
            var path = "/Home/Index.htm?q1=v1&q2=v2";

            //act
            var actual = _urlNormalizer.VerifyUrl(baseUri, path);

            //assert
            Assert.False(actual);
        }
    }
}
