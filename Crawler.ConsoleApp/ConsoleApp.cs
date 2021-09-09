using Crawler.Entities.Models;
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
        private readonly InputValidationService _inputValidationService;

        public ConsoleApp(Display display, ConsoleWrapper consoleWrapper, TestsService testsService, InputValidationService inputValidationService)
        {
            _display = display;
            _consoleWrapper = consoleWrapper;
            _testsService = testsService;
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
                    var isValidInput = await _inputValidationService.VerifyUrl(input);

                    if (!isValidInput)
                    {
                        _consoleWrapper.WtiteLine(_inputValidationService.ErrorMessage);
                    }
                    else
                    {
                        await _testsService.SaveTestResultsAsync(input);



                        int latestTestId = _testsService
                                .GetAllTests()
                                .Max(t => t.Id);

                        IEnumerable<TestDetail> details = _testsService.GetDetailsByTestId(latestTestId);

                        IEnumerable<string> urlsFromSitemap = details
                            .Where(td => !td.InWebsite)
                            .Select(td => td.Url);
                        IEnumerable<string> urlsFromWebsite = details
                            .Where(td => !td.InSitemap)
                            .Select(td => td.Url);

                        _display.ShowTable("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site", urlsFromSitemap, "URL");
                        _display.ShowTable("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml", urlsFromWebsite, "URL");

                        IEnumerable<Ping> pings = details
                            .Select(td => new Ping
                            {
                                Url = td.Url,
                                ResponseTimeMs = td.ResponseTimeMs
                            });

                        _display.ShowTable("Timing", pings, "URL", "Timing");

                        var sitemapCount = details
                            .Where(td => td.InSitemap)
                            .Count();

                        var websiteCount = details
                            .Where(td => td.InWebsite)
                            .Count();

                        _consoleWrapper.WtiteLine($"Urls(html documents) found after crawling a website: {websiteCount}");
                        _consoleWrapper.WtiteLine($"Urls found in sitemap: {sitemapCount}");
                    }
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
