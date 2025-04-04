using Game_Net;
using Game_Net.Interfaces;

internal class Program
{
    public static async Task Main()
    {
        var httpClient = new HttpClient();


        IRestClient restClient = new RestClient(httpClient);


        restClient.setBaseAddres("https://Google.com");

        string getResponnce = await restClient.GetAsync("/api/data");
        Console.WriteLine(getResponnce);

        string jsonData = "{ \"name\": \"John\", \"age\": 30 }";
        string postResponse = await restClient.PostAsync("/api/submit", jsonData);
        Console.WriteLine("POST-ответ: " + postResponse);
    }
}