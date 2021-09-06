using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.Service.Services
{
    public class InputValidationService
    {
        private readonly HttpClient _client;

        public string ErrorMessage { get; private set; }

        public InputValidationService()
        {
            _client = new HttpClient();
            ErrorMessage = String.Empty;
        }
        public async Task<bool> VerifyUlr(string url)
        {
            if(String.IsNullOrEmpty(url))
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

            var startPageUrl = $"https://{result.DnsSafeHost}";

            try
            {
                return await TryCompareUrls(startPageUrl);
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "Unknown host";

                return false;
            }
        }

        private async Task<bool> TryCompareUrls(string url)
        {
            using (var response = await _client.GetAsync(url))
            {
                if (response != null && response.IsSuccessStatusCode)
                {
                    var inputUri = new Uri(url);
                    var startPageUri = response
                        .RequestMessage
                        .RequestUri;

                    var comparisonResult = Uri.Compare(startPageUri, inputUri, UriComponents.SchemeAndServer,
                        UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);
                    if(comparisonResult != 0)
                    {
                        ErrorMessage = "Wrong scheme or host name";

                        return false;
                    }
                    return true;
                }
            }

            return false;
        }
    }
}
