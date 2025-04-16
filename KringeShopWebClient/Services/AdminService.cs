using KringeShopLib.Model;

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

        }


    }
}
