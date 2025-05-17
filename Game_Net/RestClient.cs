using Game_Net.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<string> GetAsync(string endPoint, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endPoint);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        public async Task<string> PostAsync(string endPoint, string token, string jsonData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endPoint);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<Stream> GetStreamAsync(string endPoint, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endPoint);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> GetStreamAsync(string endPoint, string token, Dictionary<string, string>? headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endPoint);

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if(headers != null)
            {
                foreach(var kvp in headers)
                {
                    request.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
                }
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<string> PostMultipartAsync(string endPoint, string token, MultipartFormDataContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endPoint);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            request.Content = content;

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string endPoint, string token, FormUrlEncodedContent formData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endPoint)
            {
                Content = formData
            };

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }
}
