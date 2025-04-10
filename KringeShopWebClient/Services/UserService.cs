using KringeShopLib.Model;
using System.Text;
using System.Text.Json;

namespace KringeShopWebClient.Services
{
    public class UserService
    {
        private readonly HttpClient client;
        public UserService()
        {
            client= Client.HttpClient;
        }

        public async Task<UserDTO> GetUserData(string username)
        {
            try
            {
                var responce = await client.GetAsync($"Users/{username}");
                if (!responce.IsSuccessStatusCode)
                {
                    return null;
                }
                else
                {
                    return await responce.Content.ReadFromJsonAsync<UserDTO>();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateUser(UserDTO user)
        {
            try
            {
                string json=JsonSerializer.Serialize(user);
                var responce = await client.PutAsync("Users", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!responce.IsSuccessStatusCode)
                {
                    //server error
                    return false;
                }
                else
                {
                    //success
                    return true;
                }
            }
            catch (Exception ex)
            {
                //error
                return false;
            }
        }
    }
}
