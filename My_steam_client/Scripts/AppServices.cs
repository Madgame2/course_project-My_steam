using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts.Interfaces;
using My_steam_client.Scripts.Services;
using Game_Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Scripts
{
    public static class AppServices
    {
        public static ServiceProvider Provider { get; private set; }
        public static long UserId { get; set; }

        public static void Init()
        {
            var services = new ServiceCollection();

            services.AddSingleton<HttpClient>();

            services.AddSingleton<Game_Net.ComunitationMannageer>(provider =>
            {
                var httpClient = provider.GetRequiredService<HttpClient>();
                return new Game_Net.ComunitationMannageer(httpClient);
            });

            services.AddSingleton<IPingService_client, client_PingService>(provider =>
            {
                var commManager = provider.GetRequiredService<Game_Net.ComunitationMannageer>();
                return new client_PingService(commManager);
            });

            services.AddSingleton<Game_Net.AuthService>(provider =>
            {
                var commManager = provider.GetRequiredService<Game_Net.ComunitationMannageer>();
                return new AuthService(commManager);
            });

            services.AddSingleton<Game_Net.StoreServices>(provider =>
            {
                var commManager = provider.GetRequiredService<Game_Net.ComunitationMannageer>();
                return new StoreServices(commManager);
            });
            services.AddSingleton<Game_Net.ResourcesService>(provider =>
            {
                var commManager = provider.GetRequiredService<Game_Net.ComunitationMannageer>();
                return new ResourcesService(commManager);
            });

            services.AddSingleton<LibMannager>();

            Provider = services.BuildServiceProvider();
        }
    }
}
