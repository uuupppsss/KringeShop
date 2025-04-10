﻿using KringeShopLib.Model;
using KringeShopWebClient.Model;
using System.Text.Json;
using System.Text;

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
            catch (Exception ex)
            {

            }
        }
    }
}
