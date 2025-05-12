using KringeShopLib.Model;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using System.Text.Json;
using KringeShopWebClient.Model;
using KringeShopWebClient.Extention;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.WebSockets;

namespace KringeShopWebClient.Services
{
    public class ConnectionService
    {
        private readonly HubConnection connection;
        private readonly HttpClient client;
        public OperationResult CurrentOperationResult {  get; private set; }


        public ConnectionService()
        {
            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5216/clientshub")
               .Build();

            client = Client.HttpClient;
        }

        public async Task<List<ProductDTO>> GetProductsList(int loadedItemsCount)
        {
            try
            {
                return await client.GetFromJsonAsync<List<ProductDTO>>($"Products/All/{loadedItemsCount}");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> GetProductsTotalCount()
        {
            try
            {
                return await client.GetFromJsonAsync<int>("Products/TotalCount");
            }
            catch
            {
                return 0;
            }
        }

        //public async Task<int> GetTotalProductsFilteredCount(string filterword)
        //{
        //    try
        //    {
        //        return await client.GetFromJsonAsync<int>("Products/FilteredCount");
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}

        //public async Task<List<ProductDTO>> GetFilteredProductsList(string filterword)
        //{
        //    try
        //    {
        //        var responce = await client.GetAsync($"Products/Filter/{filterword}");
        //        if (!responce.IsSuccessStatusCode)
        //        {
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = false,
        //                Message = "Ошибка сервера: " + responce.StatusCode.ToString()
        //            };
        //            return null;
        //        }
        //        else
        //        {
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = true
        //            };
        //            return await responce.Content.ReadFromJsonAsync<List<ProductDTO>>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CurrentOperationResult = new OperationResult()
        //        {
        //            IsSuccess = false,
        //            Message = "Ошибка: " + ex.Message
        //        };
        //        return null;
        //    }
        //}

    }
}
