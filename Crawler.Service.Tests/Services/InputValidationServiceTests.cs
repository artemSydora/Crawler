using Crawler.Service.Services;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using System.Net.Http;
using Crawler.Logic;

namespace Crawler.Service.Tests.Services
{
    public class InputValidationServiceTests
    {
        private readonly InputValidationService _inputValidationService;
        private readonly Mock<ContentLoader> _mockContentLoader;

        public InputValidationServiceTests()
        {         
            _mockContentLoader = new Mock<ContentLoader>();
            _inputValidationService = new InputValidationService(_mockContentLoader.Object);
        }

        [Fact(Timeout = 1000)]
        public async Task VerifyUlr_InputUrlIsNullOrEmpty_SetErrorMessageAndReturnFalse()
        {
            //arrange
            var inputUrl = String.Empty;
            _mockContentLoader
                .Setup(cl => cl.GetRequestUri(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Uri>());
            
            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            _mockContentLoader.Verify(cl => cl.GetRequestUri(It.IsAny<string>()), Times.Never);

            Assert.False(actualVerifyStatus);
            Assert.Equal("Input url cannot be empty", actualErrorMessage);
        }

        [Fact(Timeout = 1000)]
        public async Task VerifyUrl_InputUrlWrongUriFormat_SetErrorMessageAndReturnFalse()
        {
            //arrange
            var inputUrl = "qwe/rty.com";

            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            _mockContentLoader.Verify(cl => cl.GetRequestUri(It.IsAny<string>()), Times.Never);

            Assert.False(actualVerifyStatus);
            Assert.Equal("Wrong url format", actualErrorMessage);
        }

        [Fact(Timeout = 1000)]
        public async Task VerifyUrl_InputNotExistedUrl_SetErrorMessageAndReturnFalse()
        {
            //arrange
            var inputUrl = "https://mnbvcxz.com";
            var response = _mockContentLoader
                .Setup(x => x.GetRequestUri(It.IsAny<string>()))
                .Throws(new HttpRequestException());

            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            _mockContentLoader.Verify(cl => cl.GetRequestUri(It.IsAny<string>()), Times.Once);

            Assert.False(actualVerifyStatus);
            Assert.Equal("Unknown host", actualErrorMessage);
        }

        [Fact(Timeout = 1000)]
        public async Task VerifyUrl_InputUrlDoesNotMatchWithRequestUri_SetErrorMessageAndReturnFalse()
        {
            //arrange
            var inputUrl = "https://google.com";
            var expectedUri = _mockContentLoader
                .Setup(cl => cl.GetRequestUri(It.IsAny<string>()))
                .ReturnsAsync(new Uri("https://www.google.com"));

            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            _mockContentLoader.Verify(cl => cl.GetRequestUri(It.IsAny<string>()), Times.Once);

            Assert.False(actualVerifyStatus);
            Assert.Equal("Wrong scheme or host name", actualErrorMessage);
        }

        [Fact(Timeout = 1000)]
        public async Task VerifyUrl_InputUrlDoesMatchWithRequestUri_ReturnTrue()
        {
            //arrange
            var inputUrl = "https://www.google.com";
            var expectedUri = _mockContentLoader
                .Setup(cl => cl.GetRequestUri(It.IsAny<string>()))
                .ReturnsAsync(new Uri("https://www.google.com"));

            //actual
            var actualVerifyStatus = await _inputValidationService.VerifyUlr(inputUrl);
            var actualErrorMessage = _inputValidationService.ErrorMessage;

            //assert
            _mockContentLoader.Verify(cl => cl.GetRequestUri(It.IsAny<string>()), Times.Once);

            Assert.True(actualVerifyStatus);
            Assert.Equal(String.Empty, actualErrorMessage);
        }
    }
}
