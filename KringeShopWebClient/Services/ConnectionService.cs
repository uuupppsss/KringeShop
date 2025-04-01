using KringeShopLib.Model;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using System.Text.Json;
using KringeShopWebClient.Model;
using KringeShopWebClient.Extention;

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
            client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5216/api/")
            };

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
                        Message = "Ошибка сервера: " + responce.StatusCode.ToString()
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
                var responce = await client.PostAsync("Users", new StringContent(json, Encoding.UTF8, "application/json"));
                if (!responce.IsSuccessStatusCode)
                {
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess = false,
                        Message = "Ошибка сервера: " + responce.StatusCode.ToString()
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
            User current_user;
            try
            {
                var responce = await client.GetAsync($"Users/SignIn/{username}/{password}");
                if (!responce.IsSuccessStatusCode)
                {
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess = false,
                        Message = "Ошибка сервера: " + responce.StatusCode.ToString()
                    };
                }
                else
                {
                    current_user = await responce.Content.ReadFromJsonAsync<User>();
                    userService.SetCurrentUser(current_user);
                    CurrentOperationResult = new OperationResult()
                    {
                        IsSuccess = true,
                        Message = $"Авторизация прошла успешно! Добро пожаловать,{current_user.Username}"
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
