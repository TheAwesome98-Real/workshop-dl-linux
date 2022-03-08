using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SteamWorkshopDownloader.Helpers
{
    class HTTPClientHelper
    {
        private static readonly HttpClient HTTPClient = new HttpClient();
        public static async Task<dynamic> GetJsonAsync(string url, int retries = 1)
        {
            HTTPClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36");
            try
            {
                return JsonConvert.DeserializeObject<dynamic>(await HTTPClient.GetStringAsync(url));
            }
            catch (Exception ex)
            {
                if (retries != 0) return await GetJsonAsync(url, retries - 1);
                Environment.Exit(0);
                return null;
            }
        }
        public static async Task<dynamic> PostJsonAsync(string url, string body = "", int retries = 1)
        {
            HTTPClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36");
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36");
                request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
                return JsonConvert.DeserializeObject<dynamic>(await (await HTTPClient.SendAsync(request)).Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                if (retries != 0) return await PostJsonAsync(url, body, retries - 1);
                Environment.Exit(0);
                return null;
            }
        }
    }
}
