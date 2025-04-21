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

    public class PingService
    {
        private readonly ComunitationMannageer _comMannager;

        public PingService(ComunitationMannageer comMannager)
        {
            _comMannager = comMannager;
        }


        public async Task<PingDto> PingAync(int maxAttemps = 3, int timeOutMS = 3000, int retryDelayMs = 1000)
        {
            for (int i = 0; i < maxAttemps; i++)
            {
                try
                {
                    using var cts = new CancellationTokenSource(timeOutMS);

                    Task<NetResponse<PingDto>> pingTask =  _comMannager.SendMessageRest<PingDto>("api/ping", Protocol.Http);
                    var result = await Task.WhenAny(pingTask, Task.Delay(timeOutMS, cts.Token)) == pingTask ? await pingTask : throw new TimeoutException();


                    var ServaerResponce = result.data;

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
            return new PingDto { date = DateTime.Now, status=ServerStatus.Unknown };
        }
    }
}
