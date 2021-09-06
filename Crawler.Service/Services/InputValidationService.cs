using Crawler.Logic;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.Service.Services
{
    public class InputValidationService
    {
        private readonly ContentLoader _contentLoader;

        public string ErrorMessage { get; private set; }

        public InputValidationService(ContentLoader contentLoader)
        {
            ErrorMessage = String.Empty;

            _contentLoader = contentLoader;
        }

        public async Task<bool> VerifyUlr(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                ErrorMessage = "Input url cannot be empty";

                return false;
            }

            var isWellFormedUri = Uri.TryCreate(url, UriKind.Absolute, out Uri result);

            if (!isWellFormedUri)
            {
                ErrorMessage = "Wrong url format";

                return false;
            }

            return await TryCompareUrls($"https://{result.DnsSafeHost}");          
        }

        private async Task<bool> TryCompareUrls(string url)
        {
            var inputUri = new Uri(url);

            try
            {
                var startPageUri = await _contentLoader.GetRequestUri(url);
               
                var comparisonResult = Uri.Compare(startPageUri, inputUri, UriComponents.SchemeAndServer,
                UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);
                if (comparisonResult != 0)
                {
                    ErrorMessage = "Wrong scheme or host name";

                    return false;
                }

                return true;
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "Unknown host";

                return false;
            }         
        }
    }
}
