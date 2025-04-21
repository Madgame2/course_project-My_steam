using Game_Net;
using Game_Net.Interfaces;
using Game_Net_DTOLib;
using My_steam_server.DTO_models;
using System.Net.Http;

internal class Program
{
    static ComunitationMannageer comMannager;// = new ComunitationMannageer(httpClient);
    static PingService pingServis;// = new PingService(comMannager);
    static AuthService authServis;

    static bool isActive = true;

    public static async Task Main()
    {
        var httpClient = new HttpClient();


        comMannager = new ComunitationMannageer(httpClient);
        pingServis = new PingService(comMannager);
        authServis = new AuthService(comMannager);

        comMannager.addNewUrl(new ServerSettings { protocol = Protocol.Http, host = "localhost", port = "5254" });



        var result= await authServis.RegisterUser(new RegisterDto { Email="memail@gmail.com",Username="memail",Password="memail"});

        Console.WriteLine(result);

        Write_command_List();

        while (isActive)
        {
            int comnadId;
            int.TryParse(Console.ReadLine(), out comnadId);

            await Choise_comand(comnadId);
        }

    }


    private static void Write_command_List()
    {
        Console.WriteLine("0 - Write_command_list\n"
                        + "1 - Break\n"+
                          "2 - Ping");
    }

    private static async Task Choise_comand(int id)
    {
        switch (id) { 
            case 0:
                {
                    Write_command_List();
                }
            break;

            case 1:
            {
               isActive = false;
            }
            break;
                case 2:
                {
                    await Ping();
                }
                break;
        }

    }

    private static async Task  Ping()
    {
        PingDto status = await pingServis.PingAync();

        Console.WriteLine(status.status);
    }
}