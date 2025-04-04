using Game_Net.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;

        public RestClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAsync(string endPoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endPoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string endPoint, string jsonData)
        {
            var content = new StringContent(jsonData,Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(endPoint, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
