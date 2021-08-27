using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.Logic
{
    public class ContentLoader
    {
        private readonly HttpClient _client;

        public ContentLoader(HttpClient client)
        {
            _client = client;
        }

        public virtual async Task<string> GetContentAsync(string url)
        {
            string content = string.Empty;

            using (var response = await GetResponseAsync(url))
            {
                if (response != null && response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
            }

            return content;
        }

        public async Task<HttpResponseMessage> GetResponseAsync(string url)
        {
            var response = await _client.GetAsync(url);

            return response;
        }
    }
}

