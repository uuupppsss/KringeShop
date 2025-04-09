using KringeShopLib.Model;

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
    }
}
