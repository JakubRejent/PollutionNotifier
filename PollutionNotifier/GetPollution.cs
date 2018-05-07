using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BotRun
{
    class GetPollution
    {

        public async Task<string> get(string html)
        {
            return await DownloadPage(html);
        }

        private static async Task<string> DownloadPage(string url)
        {
            HttpClientHandler hch = new HttpClientHandler();
            hch.Proxy = null;
            hch.UseProxy = false;

            using (var client = new HttpClient(hch))
            {
                using (var r = await client.GetAsync(new Uri(url)))
                {
                    return await r.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
