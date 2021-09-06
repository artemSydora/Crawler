using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.Logic
{
    public class ContentLoader
    {
        private readonly HttpClient _client;

        public ContentLoader()
        {
            _client = new HttpClient();
        }

        public virtual async Task<string> GetContentAsync(string url)
        {
            string content = string.Empty;

            using (var response = await _client.GetAsync(url))
            {
                if (response != null && response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
            }

            return content;
        }

        public virtual async Task<Uri> GetRequestUri(string url)
        {
            using (var response = await _client.GetAsync(url))
            {
                if (response != null && response.IsSuccessStatusCode)
                {
                    var requestUri = response
                        .RequestMessage
                        .RequestUri;

                    return requestUri;
                }
            }

            throw new HttpRequestException();
        }
    }
}
