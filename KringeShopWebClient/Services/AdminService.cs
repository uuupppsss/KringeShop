using KringeShopLib.Model;
using System.Net.Http.Headers;
using System.Net.Http;
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

        public async Task<List<OrderDTO>> GetOrdersList(string token,int status_id)
        {
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"Orders/ByStatus/{status_id}");
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

        public async Task<OrderDTO> GetOrder(int order_id, string token)
        {
           
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"Orders/{order_id}");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);
                    return await response.Content.ReadFromJsonAsync<OrderDTO>();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null;
        }

        public async Task<List<OrderStatusDTO>> GetOrderStatuses(string token)
        {
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "OrderStatus");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);
                    return await response.Content.ReadFromJsonAsync<List<OrderStatusDTO>>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return null;
        }
        public async Task<List<OrderItemDTO>> GetOrderItems(int order_id)
        {
            try
            {

                return await client.GetFromJsonAsync<List<OrderItemDTO>>($"OrderItems/{order_id}");
            }
            catch
            {
                //ошибка
                return null;
            }


        }

        public async Task<UserDTO> GetUserData(int user_id)
        {
            try
            {
                var responce = await client.GetAsync($"Users/ById/{user_id}");
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

        public async Task UpdateOrder(string token, OrderDTO order)
        {
            if (token != null)
            {
                string json = JsonSerializer.Serialize(order);
                var request = new HttpRequestMessage(HttpMethod.Put, "Orders/");
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
