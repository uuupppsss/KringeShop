using KringeShopLib.Model;
using KringeShopWebClient.Model;
using System.Text.Json;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;

namespace KringeShopWebClient.Services
{
    public class CustomerService
    {
        private readonly HttpClient client;
        public CustomerService()
        {
            client = Client.HttpClient;
        }

        public async Task<List<BasketItemDTO>> GetUserBasket(string username)
        {
            try
            {
                var responce = await client.GetAsync($"BasketItems/GetUsersBasketItems/{username}");
                if (!responce.IsSuccessStatusCode)
                {
                    return null;
                }
                else
                {
                    return await responce.Content.ReadFromJsonAsync<List<BasketItemDTO>>();
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task AddProductToBasket(string username, BasketItemDTO basketItem)
        {
            try
            {
                string json = JsonSerializer.Serialize(basketItem);
                var responce = await client.PostAsync($"BasketItems/{username}", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!responce.IsSuccessStatusCode)
                {

                }
                else
                {
                    //успех
                };
            }
            catch 
            {

            }
        }

        public async Task RemoveBasketItem(int id)
        {
            try
            {
                var responce = await client.DeleteAsync($"BasketItems/{id}");
                if (!responce.IsSuccessStatusCode)
                {
                    //ошибка
                }
                else
                {
                    //успех
                };
            }
            catch
            {
                //ошибка
            }
        }

        public async Task UpdateBasketItem(BasketItemDTO basketItem)
        {
            try
            {
                string json = JsonSerializer.Serialize(basketItem);
                var responce = await client.PutAsync($"BasketItems", new StringContent(json, Encoding.UTF8, "application/json"));

                if (!responce.IsSuccessStatusCode)
                {

                }
                else
                {

                    //успех
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<int> GetBasketItemMaxCount(int basketItem_id)
        {
            try
            {
                var responce = await client.GetAsync($"BasketItems/GetMaxCount/{basketItem_id}");

                if (!responce.IsSuccessStatusCode)
                {
                    return 0;
                }
                else
                {
                    return await responce.Content.ReadFromJsonAsync<int>();
                    //успех
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<HashSet<int>> GetProductsInBasket(string username)
        {
            List<BasketItemDTO> basket = await GetUserBasket(username);
            HashSet<int> result = new HashSet<int>();
            if (basket != null)
            {
                foreach (var product in basket)
                {
                    result.Add(product.ProductId);
                }
            }
            return result;
        }

        public async Task<ProductDTO> GetProductDetails(int product_id)
        {
            try
            {
                var responce = await client.GetAsync($"Products/{product_id}");
                if (!responce.IsSuccessStatusCode)
                {
                    return null;
                }
                else
                {
                    return await responce.Content.ReadFromJsonAsync<ProductDTO>();
                    //успех
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task CreateOrder(string token,OrderDTO order)
        {
            if (token != null)
            {
                string json = JsonSerializer.Serialize(order);
                var request = new HttpRequestMessage(HttpMethod.Post, "Orders/Create");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    await client.SendAsync(request);
                }
                catch
                {
                    
                }

            }

        }

        

    }
}
