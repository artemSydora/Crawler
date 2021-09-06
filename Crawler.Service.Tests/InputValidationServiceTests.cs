using Crawler.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Crawler.Service.Tests
{
    public class InputValidationServiceTests
    {
        private readonly InputValidationService _inputValidationService;

        public InputValidationServiceTests()
        {
            _inputValidationService = new InputValidationService();
        }

        [Fact]
        public async Task VerifyUlr_InputUrlIsNullOrEmpty_SetErrorMessageAndReturnFalse()
        {
            //arrange
            var inputUrl = String.Empty;

            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            Assert.False(actualVerifyStatus);
            Assert.Equal("Input url cannot be empty", actualErrorMessage);
        }

        [Fact]
        public async Task VerifyUrl_InputUrlInWrongUriFormat_SetErrorMessageAndReturnFalse()
        {
            //arrange
            var inputUrl = "qwe/rty.com";

            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            Assert.False(actualVerifyStatus);
            Assert.Equal("Wrong url format", actualErrorMessage);
        }

        [Fact]
        public async Task VerifyUrl_HostIsUnknown_SetErrorMessageAndReturnFalse()
        {
            //arrange
            var inputUrl = "https://mnbvcxz.com";

            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            Assert.False(actualVerifyStatus);
            Assert.Equal("Unknown host", actualErrorMessage);
        }

        [Fact]
        public async Task VerifyUrl_InputUrlNotMatchWithUrlInResponse_SetErrorMessageAndReturnFalse()
        {
            //arrange
            var inputUrl = "https://google.com";

            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            Assert.False(actualVerifyStatus);
            Assert.Equal("Wrong scheme or host name", actualErrorMessage);
        }

        [Fact]
        public async Task VerifyUrl_InputUrlDoesMatchWithResponseUrl_ReturnTrue()
        {
            //arrange
            var inputUrl = "https://www.google.com";

            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            Assert.True(actualVerifyStatus);
            Assert.Equal(String.Empty, actualErrorMessage);
        }
    }
}
