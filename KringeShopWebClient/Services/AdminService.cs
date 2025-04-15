namespace KringeShopWebClient.Services
{
    public class AdminService
    {
        private readonly HttpClient client;
        public AdminService()
        {
            client = Client.HttpClient;
        }
        //public async Task<string?> UploadProductImage(MultipartFormDataContent content)
        //{
        //    //var response = await client.PostAsync("Products/Upload", content);
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    return await response.Content.ReadAsStringAsync();
        //    //}
        //    //else return null;
        //}
    }
}
