using Crawler.Logic.Crawlers.Sitemap;
using System;
using Xunit;

namespace WebsitePerformanceTool.Tests
{
    public class ParserRobotsTests
    {
        private readonly ParserRobots _robotsParser;

        public ParserRobotsTests()
        {
            _robotsParser = new ParserRobots();
        }

        [Fact(Timeout = 1000)]
        public void ReadRobots_ReturnCollectionOfValidUrls()
        {
            //arrange
            var fakeRobotsTxt = @"User-agent: Googlebot
                                  Disallow: / nogooglebot /

                                  User - agent: *
                                  Allow: /

                                  Sitemap: http://www.example.com/sitemap1.xml
                                  Sitemap: http://www.example.com/sitemap2.xml";
            //act 
            var actual = _robotsParser.ReadRobots(fakeRobotsTxt);

            //assert
            Assert.Collection(actual,
                              url => Assert.Equal("http://www.example.com/sitemap1.xml", url),
                              url => Assert.Equal("http://www.example.com/sitemap2.xml", url));
        }

        [Fact(Timeout = 1000)]
        public void ReadRobots_ShouldIgnoreUrlsPointsToArchives()
        {
            //arrange
            var fakeRobotsTxt = @"User-agent: Googlebot
                                  Disallow: / nogooglebot /

                                  User - agent: *
                                  Allow: /

                                  Sitemap: http://www.example.com/sitemap1-archive.xml
                                  Sitemap: http://www.example.com/sitemap2.xml";
            //act 
            var actual = _robotsParser.ReadRobots(fakeRobotsTxt);

            //assert
            Assert.Collection(actual,
                              url => Assert.Equal("http://www.example.com/sitemap2.xml", url));
        }

        [Fact(Timeout = 1000)]
        public void ReadRobots_ReturnCollectionOfUrlsOnlyWithXmlExtension()
        {
            //arrange
            var fakeRobotsTxt = @"User-agent: Googlebot
                                  Disallow: / nogooglebot /

                                  User - agent: *
                                  Allow: /
                                  
                                  Sitemap: http://www.example.com/sitemap1/
                                  Sitemap: http://www.example.com/sitemap2.html
                                  Sitemap: http://www.example.com/sitemap3.xml";
            //act 
            var actual = _robotsParser.ReadRobots(fakeRobotsTxt);

            //assert
            Assert.Collection(actual,
                              url => Assert.Equal("http://www.example.com/sitemap3.xml", url));
        }

        [Fact(Timeout = 1000)]
        public void ReadRobots_ReturnCollectionOfUrlsOnlyWithDns()
        {
            //arrange
            var fakeRobotsTxt = @"User-agent: Googlebot
                                  Disallow: / nogooglebot /

                                  User - agent: *
                                  Allow: /
                                  
                                  Sitemap: sitemap1.xml
                                  Sitemap: www.example.com/sitemap2.xml
                                  Sitemap: http://www.example.com/sitemap3.xml";
            //act 
            var actual = _robotsParser.ReadRobots(fakeRobotsTxt);

            //assert
            Assert.Collection(actual,
                              url => Assert.Equal("http://www.example.com/sitemap3.xml", url));
        }

        [Fact(Timeout = 1000)]
        public void ReadRobots_ContentIsNull_ReturnEmptyArray()
        {
            //act 
            var actualNull = _robotsParser.ReadRobots(null);

            //assert
            Assert.Empty(actualNull);
        }

        [Fact(Timeout = 1000)]
        public void ReadRobots_ContentIsEmpty_ReturnEmptyArray()
        {
            //act 
            var actualEmpty = _robotsParser.ReadRobots(String.Empty);

            //assert
            Assert.Empty(actualEmpty);
        }
    }
}
