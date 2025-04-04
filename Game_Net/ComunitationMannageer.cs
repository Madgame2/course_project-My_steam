using Game_Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net
{
    internal class ComunitationMannageer
    {
        private readonly IRestClient _restClient;

        private string ServerURI = "localhost:5254";


        public string ServerAddres
        {
            get => ServerAddres;
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

        public ComunitationMannageer() {
            HttpClient newClient = new HttpClient();

            _restClient = new RestClient(newClient);
        }
    }
}
