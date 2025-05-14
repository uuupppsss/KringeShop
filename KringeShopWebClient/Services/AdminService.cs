using KringeShopLib.Model;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace KringeShopWebClient.Services
{
    public class AdminService
    {
        //для админа
        private readonly HttpClient client;
        public AdminService()
        {
            client = Client.HttpClient;
        }
       
        public async Task AddProduct(string token, ProductDTO product, List<byte[]> images)
        {
            
           if(token!=null)
           {
                HttpResponseMessage responce=null;
                int addedProductId = 0;
                string json = JsonSerializer.Serialize(product);
                var request = new HttpRequestMessage(HttpMethod.Post, "Products/");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    responce=await client.SendAsync(request);
                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                }
                if(responce!=null&&responce.IsSuccessStatusCode)
                {
                    addedProductId = await responce.Content.ReadFromJsonAsync<int>();
                    string images_json = JsonSerializer.Serialize(images);
                    var request2 = new HttpRequestMessage(HttpMethod.Post, $"Products/Images/{addedProductId}");
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                try
                {
                    await client.SendAsync(request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
        
        public async Task<List<OrderItemDTO>> GetOrderItems(string token, int order_id)
        {
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"OrderItems/{order_id}");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.SendAsync(request);
                    return await response.Content.ReadFromJsonAsync<List<OrderItemDTO>>();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
            return null;


        }

        public async Task<UserDTO> GetUserData(string token, int user_id)
        {
            if (token != null)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"Users/ById/{user_id}");
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
