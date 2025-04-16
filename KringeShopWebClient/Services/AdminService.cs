using KringeShopLib.Model;
using System.Text;
using System.Text.Json;

namespace KringeShopWebClient.Services
{
    public class AdminService
    {
        private readonly HttpClient client;
        public AdminService()
        {
            client = Client.HttpClient;
        }
       
        public async Task AddProduct(ProductDTO product, List<byte[]> images)
        {
            
            try
            {
                int addedProductId=0;
                string json = JsonSerializer.Serialize(product);
                var responce = await client.PostAsync("Products", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!responce.IsSuccessStatusCode)
                {
                  //ошибка
                }
                else
                {
                    addedProductId = await responce.Content.ReadFromJsonAsync<int>();
                    string imajes_json = JsonSerializer.Serialize(images);
                    var images_responce = await client.PostAsync($"Products/Images{addedProductId}", new StringContent(imajes_json, Encoding.UTF8, "application/json"));
                    if (!responce.IsSuccessStatusCode)
                    {
                        //ошибка
                    }
                    else
                    {
                        //успех
                    }
                }
            }
            catch
            {

            }
        }


    }
}
