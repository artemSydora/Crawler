using Crawler.Logic.Models;
using Crawler.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.ConsoleApp
{
    public class ConsoleApp
    {
        private readonly Display _display;
        private readonly ConsoleWrapper _consoleWrapper;
        private readonly TestsService _testsService;
        private readonly DetailsService _detailsService;
        private readonly InputValidationService _inputValidationService;

        public ConsoleApp(Display display, ConsoleWrapper consoleWrapper, TestsService testsService, DetailsService detailsService, InputValidationService inputValidationService)
        {
            _display = display;
            _consoleWrapper = consoleWrapper;
            _testsService = testsService;
            _detailsService = detailsService;
            _inputValidationService = inputValidationService;
        }

        public async Task Run()
        {
            _consoleWrapper.WtiteLine("Please, input website URL or press <Enter> to exit...");
            var input = _consoleWrapper.ReadLine();

            while (!String.IsNullOrEmpty(input))
            {
                try
                {
                    var isValidInput = await _inputValidationService.VerifyUlr(input);

                    if (!isValidInput)
                    {
                        _consoleWrapper.WtiteLine("Invalid input url");
                    }
                    else
                    {
                        await _testsService.SaveTestResults(input);
                       
                    }
                
                        var latestTestId = _testsService
                            .GetAllTests()
                            .Max(t => t.Id);
                    
                    IEnumerable<string> urlsFromSitemap = _detailsService.GetUniqueSitemapUrlsByTestId(latestTestId);
                    IEnumerable<string> urlsFromWebsite = _detailsService.GetUniqueWebsiteUrlsByTestId(latestTestId);

                    _display.ShowTable("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site", urlsFromSitemap, "URL");
                    _display.ShowTable("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml", urlsFromWebsite, "URL");

                    IEnumerable<Ping> pings = _detailsService.GetOrderedPingResultsByTestId(latestTestId);

                    _display.ShowTable("Timing", pings, "URL", "Timing");

                    (int sitemapCount, int websiteCount) counts = _detailsService.GetUrlCounts(latestTestId);

                    _consoleWrapper.WtiteLine($"Urls(html documents) found after crawling a website: {counts.websiteCount}");
                    _consoleWrapper.WtiteLine($"Urls found in sitemap: {counts.sitemapCount}");
                }
                catch (ArgumentException ex)
                {
                    _consoleWrapper.WtiteLine(ex.Message);
                }
                catch (HttpRequestException ex)
                {
                    _consoleWrapper.WtiteLine(ex.Message);
                }



                _consoleWrapper.WtiteLine("Please, input website URL or press <Enter> to exit...");

                input = _consoleWrapper.ReadLine();
            }

            Environment.Exit(0);
        }
    }
}
