using KringeShopLib.Model;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using System.Text.Json;
using KringeShopWebClient.Model;
using KringeShopWebClient.Extention;
using System.Net.Http.Headers;
using System.Net.Http;

namespace KringeShopWebClient.Services
{
    public class ConnectionService
    {
        private readonly HubConnection connection;
        private readonly HttpClient client;
        private readonly UserService userService;
        public OperationResult CurrentOperationResult {  get; private set; }
        //private readonly NotifyService notifyService;


        public ConnectionService(UserService userService /*, NotifyService notifyService*/)
        {
            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5216/clientshub")
               .Build();

            client = Client.HttpClient;
            this.userService = userService;
            //this.notifyService = notifyService;
        }


        //private async void Connect()
        //{
        //    string username=string.Empty;
        //    if(userService.CurrentUser.Username!=null) username = userService.CurrentUser.Username;
        //    await connection.InvokeAsync("ConnectClient", username);
        //}

        public async Task<List<Product>> GetProductsList()
        {
            try
            {
                var responce = await client.GetAsync("Products");
                if (!responce.IsSuccessStatusCode)
                {
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess = false,
                        Message = "Ошибка сервера: " + responce.StatusCode.ToString()
                    };
                    return null;
                }
                else
                {
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess=true
                    };
                    return await responce.Content.ReadFromJsonAsync<List<Product>>();
                }
            }
            catch (Exception ex)
            {
                CurrentOperationResult = new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Ошибка: " + ex.ToString()
                };
                return null;
            }
        }

        public async Task CreateOrder(List<BasketItem> productsList)
        {
            try
            {
                string json=JsonSerializer.Serialize(productsList);
                var responce = await client.PostAsync("Orders", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!responce.IsSuccessStatusCode)
                {
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess = false,
                        Message = "Ошибка сервера: " + responce.StatusCode.ToString() + await responce.Content.ReadAsStringAsync()
                    };
                }
                else CurrentOperationResult = new OperationResult()
                {
                    IsSuccess = true,
                    Message="Заказ успешно создан!"
                };
            }
            catch (Exception ex)
            {
                CurrentOperationResult = new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Ошибка: " + ex.ToString()
                };
            }
        }

        public async Task SignUp(UserDTO user)
        {
            try
            {
                user.RoleId = 2;
                string json = JsonSerializer.Serialize(user);
                var responce = await client.PostAsync("Auth", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!responce.IsSuccessStatusCode)
                {
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess = false,
                        Message = "Ошибка сервера: " + responce.StatusCode.ToString() + await responce.Content.ReadAsStringAsync()
                    };
                }
                else
                {
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess = true,
                        Message= $"Регистрация прошла успешно! Выполните вход в систему"
                    };
                }

            }
            catch (Exception ex)
            {
                CurrentOperationResult = new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Ошибка: " + ex.ToString()
                };
            }
        }

        public async Task SignIn(string username, string password)
        {
            ResponseTokenAndStuff serverResponce;
            try
            {
                var responce = await client.GetAsync($"Auth/{username}/{password}");
                if (!responce.IsSuccessStatusCode)
                {
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess = false,
                        Message = "Ошибка сервера: " + responce.StatusCode.ToString() + await responce.Content.ReadAsStringAsync()
                    };
                }
                else
                {
                    serverResponce = await responce.Content.ReadFromJsonAsync<ResponseTokenAndStuff>();
                    UserDTO authUser = new UserDTO()
                    {
                        Id=serverResponce.Id,
                        Username=username,
                        Password=password,
                        Email=serverResponce.Email,
                        ContactPhone=serverResponce.Phone
                    };
                    userService.SetCurrentUser(authUser);
                    Client.SetToken(serverResponce.Token);
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess = true,
                        Message = $"Авторизация прошла успешно! Токен - {serverResponce.Token}"
                        //$"Авторизация прошла успешно! Добро пожаловать,{authUser.Username}"
                    };
                }
            }
            catch (Exception ex)
            {
                CurrentOperationResult = new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Ошибка: " + ex.ToString()
                };
            }
        }
    }
}
