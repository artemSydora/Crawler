using System;
using Xunit;

namespace Crawler.Logic.Tests
{
    public class CustomUriComparerTests
    {
        private readonly CustomUriComparer _comparer;

        public CustomUriComparerTests()
        {
            _comparer = new CustomUriComparer();
        }

        [Fact(Timeout = 1000)]
        public void Equals_ShouldIgnoreSchemeWhenConpareUri()
        {
            //arrange
            var fakeUri1 = new Uri("http://www.example.com");
            var fakeUri2 = new Uri("https://www.example.com");

            //act
            var result = _comparer.Equals(fakeUri1, fakeUri2);

            //assert
            Assert.True(result);
        }

        [Fact(Timeout = 1000)]
        public void GetHashCode_ShouldCombineHostAndAbsolutePathHasCodes()
        {
            //act
            var actualHashCode1 = _comparer.GetHashCode(new Uri("http://www.example.com"));
            var actualHashCode2 = _comparer.GetHashCode(new Uri("https://www.example.com"));

            //assert
            Assert.Equal(actualHashCode1, actualHashCode2);
        }
    }
}
