using Microsoft.AspNetCore.SignalR.Client;

namespace KringeShopWebClient.Services
{
    public class ConnectionService
    {
        private readonly HubConnection connection;
        private readonly HttpClient client;
        public ConnectionService()
        {
            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5216/sellinghub")
               .Build();
            client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5216/api")
            };
        }
    }
}
