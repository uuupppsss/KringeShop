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
        //общедоступное

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

        public async Task<List<ProductDTO>> GetProductsList(int loadedItemsCount, string filterword = null, int type_id = 0)
        {
            if (filterword == null) filterword = "-";
            try
            {
                return await client.GetFromJsonAsync<List<ProductDTO>>
                    ($"Products/All/{loadedItemsCount}/{filterword}/{type_id}");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> GetProductsCount(string filterword = null,int type_id=0)
        {
            if(filterword==null) filterword = "-";
            try
            {
                return await client.GetFromJsonAsync<int>($"Products/Count/{filterword}/{type_id}");
            }
            catch
            {
                return 0;
            }
        }

        public async Task<ProductDTO> GetProductDetails(int product_id)
        {
            try
            {
                return await client.GetFromJsonAsync<ProductDTO>($"Products/{product_id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
