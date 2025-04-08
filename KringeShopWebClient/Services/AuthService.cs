using KringeShopLib.Model;
using KringeShopWebClient.Model;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;
using System.Text;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using KringeShopWebClient.Extention;

namespace KringeShopWebClient.Services
{
    public class AuthService
    {
        private readonly HttpClient client;
        public AuthService()
        {
            client = Client.HttpClient;
        }

        public async Task SignUp(UserDTO user)
        {
            try
            {
                user.RoleId = 2;
                string json = JsonSerializer.Serialize(user);
                var responce = await client.PostAsync("Auth", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!responce.IsSuccessStatusCode)
                {
                    
                }
                else
                {
                    
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        public async Task<ResponseTokenAndStuff> SignIn(string username, string password)
        {
            ResponseTokenAndStuff serverResponce;
            try
            {
                var responce = await client.GetAsync($"Auth/{username}/{password}");
                if (!responce.IsSuccessStatusCode)
                {
                    
                    return null;
                }
                else
                {
                    serverResponce = await responce.Content.ReadFromJsonAsync<ResponseTokenAndStuff>();
                    Client.SetToken(serverResponce.Token);
                    return serverResponce;
                }
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
    }
}
