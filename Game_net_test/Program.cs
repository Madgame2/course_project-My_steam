using Game_Net;
using Game_Net.Interfaces;

internal class Program
{
    public static async Task Main()
    {
        var httpClient = new HttpClient();


        var comMannager = new ComunitationMannageer(httpClient);
        var pingServis = new PingService(comMannager);

        comMannager.addNewUrl(new ServerSettings { protocol = Protocol.Http, host = "localhost", port = "5254" });

        ServerStatus status = await pingServis.PingAync();


        Console.WriteLine(status);
    }
}