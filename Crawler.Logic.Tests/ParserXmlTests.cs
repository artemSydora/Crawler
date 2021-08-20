using System;
using Xunit;
using Crawler.Logic.Crawlers.Sitemap;
using static Crawler.Logic.Crawlers.Sitemap.ParserXml;

namespace WebsitePerformanceTool.Tests
{
    public class ParserXmlTests
    {
        private readonly ParserXml _xmlPageParser;

        public ParserXmlTests()
        {
            _xmlPageParser = new ParserXml();
        }

        [Fact(Timeout = 1000)]
        public void ParseDocument_OptionsEqualsSitemap_ShouldParseSitemap()
        {
            //arrange
            var fakePage = @"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
                                <url>
                                    <loc>http://www.example.com/</loc>
                                    <lastmod>2016 - 11 - 21</lastmod>
                                </url>
                                <url>
                                    <loc>http://www.example.com/Home/</loc>
                                    <lastmod>2016 - 11 - 21</lastmod>
                                </url> 
                                <url>
                                    <loc>http://www.example.com/About</loc>
                                    <lastmod>2016 - 11 - 21</lastmod>
                                </url> 
                            </urlset>";

            //act
            var actual = _xmlPageParser.ParseDocument(fakePage, ParsingOptions.Sitemap);

            //assert
            Assert.Collection(actual,
                              url => Assert.Equal("http://www.example.com/", url),
                              url => Assert.Equal("http://www.example.com/Home/", url),
                              url => Assert.Equal("http://www.example.com/About", url));
        }

        [Fact(Timeout = 1000)]
        public void ParseSiteindex_OptionsEqualsSiteindex_ShouldParseSiteindex()
        {
            //arrange
            var fakePage = @"<sitemapindex xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
                                <sitemap>
                                    <loc>http://www.example.com/sitemaps/sitemap1.xml</loc>
                                    <lastmod>2016 - 11 - 21</lastmod>
                                </sitemap>
                                <sitemap>
                                    <loc>http://www.example.com/sitemaps/sitemap2.xml</loc>
                                    <lastmod>2016 - 11 - 21</lastmod>
                                </sitemap> 
                                <sitemap>
                                    <loc>http://www.example.com/sitemaps/sitemap3.xml</loc>
                                    <lastmod>2016 - 11 - 21</lastmod>
                                </sitemap> 
                            </sitemapindex>";

            //act
            var actual = _xmlPageParser.ParseDocument(fakePage, ParsingOptions.Siteindex);

            //assert
            Assert.Collection(actual,
                              url => Assert.Equal("http://www.example.com/sitemaps/sitemap1.xml", url),
                              url => Assert.Equal("http://www.example.com/sitemaps/sitemap2.xml", url),
                              url => Assert.Equal("http://www.example.com/sitemaps/sitemap3.xml", url));
        }

        [Fact(Timeout = 1000)]
        public void ParseDocument_ContentIsNull_ReturnEmptyCollection()
        {
            //act
            var actualSitemap = _xmlPageParser.ParseDocument(null, ParsingOptions.Sitemap);
            var actualSiteIndex = _xmlPageParser.ParseDocument(null, ParsingOptions.Siteindex);

            //assert
            Assert.Empty(actualSitemap);
            Assert.Empty(actualSiteIndex);
        }

        [Fact(Timeout = 1000)]
        public void ParseSitemap_ContentIsEmpty_ReturnEmptyCollection()
        {
            //act
            var actualSitemap = _xmlPageParser.ParseDocument(String.Empty, ParsingOptions.Sitemap);
            var actualSiteIndex = _xmlPageParser.ParseDocument(String.Empty, ParsingOptions.Siteindex);

            //assert
            Assert.Empty(actualSitemap);
            Assert.Empty(actualSiteIndex);
        }
    }
}
