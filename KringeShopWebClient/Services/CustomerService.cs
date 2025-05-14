using KringeShopLib.Model;
using KringeShopWebClient.Model;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace KringeShopWebClient.Services
{
    public class CustomerService
    {
        //для клиента
        private readonly HttpClient client;
        public CustomerService()
        {
            client = Client.HttpClient;
        }

        //***
        public async Task<List<BasketItemDTO>> GetUserBasket(string token)
        {
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"BasketItems/GetUsersBasketItems");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);
                    return await response.Content.ReadFromJsonAsync<List<BasketItemDTO>>();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
            return null;
        }

        //***
        public async Task AddProductToBasket(string token, BasketItemDTO basketItem)
        {
            if(token!=null)
            {
                try
                {
                    string json = JsonSerializer.Serialize(basketItem);
                    var request = new HttpRequestMessage(HttpMethod.Post, $"BasketItems");
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);
                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //***
        public async Task RemoveBasketItem(string token, int id)
        {
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Delete, $"BasketItems/{id}");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //***
        public async Task UpdateBasketItem(string token,BasketItemDTO basketItem)
        {
            if (token != null)
            {
                try
                {
                    string json = JsonSerializer.Serialize(basketItem);
                    var request = new HttpRequestMessage(HttpMethod.Put, $"BasketItems");
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //***
        public async Task<int> GetBasketItemMaxCount(string token, int basketItem_id)
        {
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"BasketItems/GetMaxCount/{basketItem_id}");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);
                    return await response.Content.ReadFromJsonAsync<int>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return 0;
        }

        
        public async Task<HashSet<int>> GetProductsInBasket(string token)
        {
            List<BasketItemDTO> basket = await GetUserBasket(token);
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

        //***
        public async Task CreateOrder(string token,OrderDTO order)
        {
            if (token != null)
            {
                try
                {
                    string json = JsonSerializer.Serialize(order);
                    var request = new HttpRequestMessage(HttpMethod.Post, "Orders/Create");
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    await client.SendAsync(request);
                }
                catch
                {
                    
                }

            }

        }

        //***
        public async Task<List<OrderDTO>> GetUserOrders(string token, int status_id)
        {
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"Orders/ByUser/{status_id}");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);
                    return await response.Content.ReadFromJsonAsync<List<OrderDTO>>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null;
        }

        //***
        public async Task<UserDTO> GetUserData(string token)
        {
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"Users");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);
                    return await response.Content.ReadFromJsonAsync<UserDTO>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null;
        }

        //***
        public async Task UpdateUser(string token, UserDTO user)
        {
            if (token != null)
            {
                try
                {
                    string json = JsonSerializer.Serialize(user);
                    var request = new HttpRequestMessage(HttpMethod.Put, "Users");
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    await client.SendAsync(request);
                }
                catch
                {

                }

            }
        }
    }

}
