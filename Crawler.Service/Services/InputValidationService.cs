using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.Service.Services
{
    public class InputValidationService
    {
        private readonly HttpClient _client;

        public InputValidationService()
        {
            _client = new HttpClient();
        }
        public async Task<bool> VerifyUlr(string url)
        {
            var isWellFormedUri = Uri.TryCreate(url, UriKind.Absolute, out Uri result);

            if (!isWellFormedUri)
            {
                return false;
            }

            var startPageUrl = $"https://{result.DnsSafeHost}";

            try
            {
                return await TryCompareUrls(startPageUrl);
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        private async Task<bool> TryCompareUrls(string url)
        {
            var startPageUrl = String.Empty;

            using (var response = await _client.GetAsync(url))
            {
                if (response != null && response.IsSuccessStatusCode)
                {
                    startPageUrl = response
                        .RequestMessage
                        .RequestUri
                        .ToString();
                }
            }

            return !String.Equals(startPageUrl, url, StringComparison.OrdinalIgnoreCase);
        }
    }
}
