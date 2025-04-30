using Game_Net.Interfaces;
using Game_Net_DTOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

            return $"{protocolStr}://{host}:{port}/{endPoint}";
        }
    }

    public class ComunitationMannageer
    {
        private readonly IRestClient _restClient;

        private string ServerURI = "localhost:5254";


        private Dictionary<Protocol, ServerSettings> ServerUrls = new Dictionary<Protocol, ServerSettings>();


        public string JWT_token = string.Empty;
        public string RefrashToken = string.Empty;
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
        public async Task<NetResponse<T>> SendMessageRest<T>(string endpoint,Protocol protocol, string jsonData) {

            if (ServerUrls.TryGetValue(protocol, out var settings))
            {

                string fullUrl = settings.fullUrl(endpoint);

                    if (string.IsNullOrEmpty(jsonData))
                    {
                        string json = await _restClient.GetAsync(fullUrl,JWT_token);

                        var result = JsonSerializer.Deserialize<NetResponse<T>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return result;
                    }
                    else
                    {
                        string json = await _restClient.PostAsync(fullUrl, JWT_token, jsonData);

                        var result = JsonSerializer.Deserialize<NetResponse<T>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return result;
                    }

            }
            else
            {
                throw new UndefinedProtocolException(protocol,"");
            }
        }

        public async Task<NetResponse<T>> SendMessageRest<T>(string endpoint, Protocol protocol)
        {

            if (ServerUrls.TryGetValue(protocol, out var settings))
            {
                string fullUrl = settings.fullUrl(endpoint);


                    string json = await _restClient.GetAsync(fullUrl, JWT_token);

                    var result = JsonSerializer.Deserialize<NetResponse<T>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return result;


            }
            else
            {
                throw new UndefinedProtocolException(protocol, "");
            }
        }
    }
}
