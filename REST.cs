using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace scrapi
{
    internal static class REST
    {
        public static async Task<T> GET<T>(string url,string id)
        {
            var client = new RestClient(string.Concat(url,id));
            var request = new RestRequest();

            var response = await client.ExecuteGetAsync(request);
            var content = response.Content;

            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;

            return JsonConvert.DeserializeObject<T>(content);
        }
        public static async Task<T> GET<T>(string url)
        {
            var client = new RestClient(string.Concat(url));
            var request = new RestRequest();

            var response = await client.ExecuteGetAsync(request);
            var content = response.Content;

            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
