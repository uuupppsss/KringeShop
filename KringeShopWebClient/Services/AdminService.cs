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
                    string images_json = JsonSerializer.Serialize(images);
                    var images_responce = await client.PostAsync($"Products/Images/{addedProductId}", new StringContent(images_json, Encoding.UTF8, "application/json"));
                    if (!images_responce.IsSuccessStatusCode)
                    {
                        string error= await images_responce.Content.ReadAsStringAsync();
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

        public async Task<List<ProductTypeDTO>> GetTypesList()
        {
            try
            {
                var responce = await client.GetAsync("ProductTypes");
                if (!responce.IsSuccessStatusCode)
                {
                    //error
                    return null;
                }
                else
                {
                    //success
                    return await responce.Content.ReadFromJsonAsync<List<ProductTypeDTO>>();
                }
            }
            catch
            {
                //error
                return null;
            }
        }

        public async Task<List<OrderDTO>> GetOrdersList(int status_id)
        {
            try
            {
                var responce = await client.GetAsync($"Orders/ByStatus/{status_id}");
                if (!responce.IsSuccessStatusCode)
                {
                    //error
                    return null;
                }
                else
                {
                    //success
                    return await responce.Content.ReadFromJsonAsync<List<OrderDTO>>();
                }
            }
            catch
            {
                //error
                return null;
            }
        }

    }
}
