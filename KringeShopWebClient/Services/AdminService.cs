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

        public async Task<OrderDTO> GetOrder(int order_id)
        {
            try
            {
                var responce = await client.GetAsync($"Orders/{order_id}");
                if (!responce.IsSuccessStatusCode)
                {
                    //error
                    return null;
                }
                else
                {
                    //success
                    return await responce.Content.ReadFromJsonAsync<OrderDTO>();
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<OrderStatusDTO>> GetOrderStatuses(string token)
        {
            if (token != null)
            {
                //var request = new HttpRequestMessage(HttpMethod.Get, "OrderStatus");
                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //var response = await client.SendAsync(request);
                //return await response.Content.ReadFromJsonAsync<List<OrderStatusDTO>>();
                using (var client=new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri("http://localhost:5216/api/");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        return await client.GetFromJsonAsync<List<OrderStatusDTO>>("OrderStatus");
                    }
                    catch
                    {
                        //ошибка
                    }
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

        public async Task UpdateOrder(OrderDTO order)
        {
            try
            {

            }
            catch
            {

            }
        }
    }
}
