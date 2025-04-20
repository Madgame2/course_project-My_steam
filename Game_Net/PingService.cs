using Game_Net_DTOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Game_Net
{
    public enum ServerStatusEnum
    {
        Unknown,
        Online,
        Maintenance,
        Ofline
    }

    public class ServerStatus
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ServerStatusEnum Status { get; set; }
        public string? Message {  get; set; }
        public DateTime TimesTamp { get; set; }
    }

    public class PingService
    {
        private readonly ComunitationMannageer _comMannager;

        public PingService(ComunitationMannageer comMannager)
        {
            _comMannager = comMannager;
        }


        public async Task<ServerStatus> PingAync(int maxAttemps = 3, int timeOutMS = 3000, int retryDelayMs = 1000)
        {
            for (int i = 0; i < maxAttemps; i++)
            {
                try
                {
                    using var cts = new CancellationTokenSource(timeOutMS);

                    Task<NetResponse<string>> pingTask =  _comMannager.SendMessageRest<string>("ping",Protocol.Http);
                    var result = await Task.WhenAny(pingTask, Task.Delay(timeOutMS, cts.Token)) == pingTask ? await pingTask : throw new TimeoutException();


                    var ServaerResponce = JsonSerializer.Deserialize<ServerStatus>(result.data);

                    if (ServaerResponce != null) return ServaerResponce;
                }
                catch (TimeoutException)
                {
                    Console.WriteLine($"Ping {i}: time out...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                }

                if (i < maxAttemps)
                {
                    await Task.Delay(retryDelayMs); // Задержка перед новой попыткой
                }
            }

            Console.WriteLine("Server not responce");
            return new ServerStatus { Status = ServerStatusEnum.Ofline, Message = "can't access the server" };
        }
    }
}
