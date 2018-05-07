using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BotRun
{
    class Geocode
    {
        private string generateRequest(string city, string province, string street)
        {
            string startHtml = "https://maps.googleapis.com/maps/api/geocode/json?address=Poland+";
            string apiKey = "[APIKEY]";
            return startHtml + city + "+" + province + "+" + street + "&key=" + apiKey;
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
                    string result = await r.Content.ReadAsStringAsync();
                    return result;
                }
            }
        }

        public string getLan(string city, string province, string street)
        {
            string request = generateRequest(city, province, street);
            Task<string> result = DownloadPage(request);
            if (!String.IsNullOrWhiteSpace(result.Result))
            {
                JObject o = JObject.Parse(result.Result);
                return (string)o.SelectToken("results[0].geometry.location.lat");
            }
            return null;
        }

        public string getLng(string city, string province, string street)
        {
            string request = generateRequest(city, province, street);
            Task<string> result = DownloadPage(request);
            if (!String.IsNullOrWhiteSpace(result.Result))
            {
                JObject o = JObject.Parse(result.Result);
                return (string)o.SelectToken("results[0].geometry.location.lng");
            }
            return null;
        }
    }
}
