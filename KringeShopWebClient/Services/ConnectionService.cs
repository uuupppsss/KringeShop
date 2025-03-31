using KringeShopLib.Model;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using System.Text.Json;

namespace KringeShopWebClient.Services
{
    public class ConnectionService
    {
        private readonly HubConnection connection;
        private readonly HttpClient client;
        private readonly UserService userService;
        public ConnectionService()
        {
            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5216/clientshub")
               .Build();
            client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5216/api")
            };

            
            
        }

        private async void Connect()
        {
            string username=string.Empty;
            if(userService.CurrentUser.Username!=null) username = userService.CurrentUser.Username;
            await connection.InvokeAsync("ConnectClient", username);
        }

        public async Task<List<Product>> GetProducsList()
        {
            try
            {
                var responce = await client.GetAsync("Products");
                if (!responce.IsSuccessStatusCode)
                {
                    return null;
                }
                else return await responce.Content.ReadFromJsonAsync<List<Product>>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task CreateOrder(List<BasketItem> productsList)
        {
            try
            {
                string json=JsonSerializer.Serialize(productsList);
                var responce = await client.PostAsync("Orders", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!responce.IsSuccessStatusCode)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SignUp(User user)
        {
            User current_user= null;
            try
            {
                string json = JsonSerializer.Serialize(user);
                var responce = await client.PostAsync("Users", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!responce.IsSuccessStatusCode)
                {

                }
                else current_user=await responce.Content.ReadFromJsonAsync<User>();
                userService.SetCurrentUser(current_user);

            }
            catch (Exception ex)
            {

            }
        }

        public async Task SignIn(string username, string password)
        {
            User current_user = null;
            try
            {
                var responce = await client.GetAsync($"Users/SignIn/{username}/{password}");
                if (!responce.IsSuccessStatusCode)
                {

                }
                else current_user = await responce.Content.ReadFromJsonAsync<User>();
                userService.SetCurrentUser(current_user);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
