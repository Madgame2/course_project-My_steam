using Game_Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net
{
    public enum Protocol
    {
        Http,
        Https
    }

    public class ServerSettings{
        public Protocol protocol { get; set; } = Protocol.Http;
        public string host { get; set; }
        public string port { get; set; }

        public string fullUrl(string endPoint)
        {
            string protocolStr = protocol == Protocol.Http ? "Http" : "Https";

            return $"{protocolStr}://{host}/{endPoint}";
        }
    }

    public class ComunitationMannageer
    {
        private readonly IRestClient _restClient;

        private string ServerURI = "localhost:5254";


        private Dictionary<Protocol, ServerSettings> ServerUrls = new Dictionary<Protocol, ServerSettings>();

        public void addNewUrl(ServerSettings newSettings)
        {
            if (ServerUrls.ContainsKey(newSettings.protocol))
                throw new Exception($"{newSettings.protocol} already defined");

            ServerUrls[newSettings.protocol] = newSettings;  
        }

        public string ServerAddres
        {
            get => ServerURI;
            set {
                if (!string.IsNullOrEmpty(value))
                {
                    ServerAddres = value;
                }
                else
                {
                    throw new ArgumentException("Server address cannot be null or whitespace.");
                }
            } 
        }

        public ComunitationMannageer(HttpClient httpClient) {

            _restClient = new RestClient(httpClient);
        }


        /// <summary>
        /// Отправляет сообщение на сервер через REST.
        /// GET — если jsonData пустой; POST — если jsonData задан.
        /// </summary>
        public async Task<string> SendMessageRest(string endpoint,Protocol protocol, string jsonData) {

            if (ServerUrls.TryGetValue(protocol, out var settings))
            {

                string fullUrl = settings.fullUrl(endpoint);

                if (string.IsNullOrEmpty(jsonData))
                {
                    return await _restClient.GetAsync(fullUrl);
                }
                else
                {
                    return await _restClient.PostAsync(fullUrl, jsonData);
                }
            }
            else
            {
                throw new Exception($"No such protocol {protocol}");
            }
        }

        public async Task<string> SendMessageRest(string endpoint, Protocol protocol)
        {
            if (ServerUrls.TryGetValue(protocol, out var settings))
            {
                string fullUrl = settings.fullUrl(endpoint);

                return await _restClient.GetAsync(fullUrl);
            }
            else
            {
                throw new Exception($"No such protocol {protocol}");
            }
        }
    }
}
