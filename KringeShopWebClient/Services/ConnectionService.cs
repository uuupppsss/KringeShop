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
        //private readonly NotifyService notifyService;


        public ConnectionService()
        {
            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5216/clientshub")
               .Build();

            client = Client.HttpClient;
            //this.notifyService = notifyService;
        }


        //private async void Connect()
        //{
        //    string username=string.Empty;
        //    if(userService.CurrentUser.Username!=null) username = userService.CurrentUser.Username;
        //    await connection.InvokeAsync("ConnectClient", username);
        //}

        public async Task<List<ProductDTO>> GetProductsList()
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
                    return await responce.Content.ReadFromJsonAsync<List<ProductDTO>>();
                }
            }
            catch (Exception ex)
            {
                CurrentOperationResult = new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Ошибка: " + ex.Message
                };
                return null;
            }
        }

        public async Task<List<ProductDTO>> GetFilteredProductsList(string filterword)
        {
            try
            {
                var responce = await client.GetAsync($"Products/Filter/{filterword}");
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
                        IsSuccess = true
                    };
                    return await responce.Content.ReadFromJsonAsync<List<ProductDTO>>();
                }
            }
            catch (Exception ex)
            {
                CurrentOperationResult = new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Ошибка: " + ex.Message
                };
                return null;
            }
        }

        //public async Task CreateOrder(List<BasketItem> productsList)
        //{
        //    try
        //    {
        //        string json=JsonSerializer.Serialize(productsList);
        //        var responce = await client.PostAsync("Orders", new StringContent(json, Encoding.UTF8, "application/json"));
        //        if (!responce.IsSuccessStatusCode)
        //        {
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = false,
        //                Message = "Ошибка сервера: " + responce.StatusCode.ToString() + await responce.Content.ReadAsStringAsync()
        //            };
        //        }
        //        else CurrentOperationResult = new OperationResult()
        //        {
        //            IsSuccess = true,
        //            Message="Заказ успешно создан!"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        CurrentOperationResult = new OperationResult()
        //        {
        //            IsSuccess = false,
        //            Message = "Ошибка: " + ex.Message
        //        };
        //    }
        //}

       

        //public async Task<List<BasketItemDTO>> GetBasketItems(int user_id)
        //{
        //    try
        //    {
        //        var responce = await client.GetAsync($"BasketItems/GetUsersBasketItems/{user_id}");
        //        if (!responce.IsSuccessStatusCode)
        //        {
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = false,
        //                Message = "Ошибка сервера: " + responce.StatusCode.ToString() + await responce.Content.ReadAsStringAsync()
        //            };
        //            return null;
        //        }
        //        else
        //        {
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = true,
        //            };
        //            return await responce.Content.ReadFromJsonAsync<List<BasketItemDTO>>();
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

        //public async Task SignUp(UserDTO user)
        //{
        //    try
        //    {
        //        user.RoleId = 2;
        //        string json = JsonSerializer.Serialize(user);
        //        var responce = await client.PostAsync("Auth", new StringContent(json, Encoding.UTF8, "application/json"));
        //        if (!responce.IsSuccessStatusCode)
        //        {
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = false,
        //                Message = "Ошибка сервера: " + responce.StatusCode.ToString() + await responce.Content.ReadAsStringAsync()
        //            };
        //        }
        //        else
        //        {
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = true,
        //                Message= $"Регистрация прошла успешно! Выполните вход в систему"
        //            };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        CurrentOperationResult = new OperationResult()
        //        {
        //            IsSuccess = false,
        //            Message = "Ошибка: " + ex.Message
        //        };
        //    }
        //}

        //public async Task<UserDTO> SignIn(string username, string password)
        //{
        //    ResponseTokenAndStuff serverResponce;
        //    try
        //    {
        //        var responce = await client.GetAsync($"Auth/{username}/{password}");
        //        if (!responce.IsSuccessStatusCode)
        //        {
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = false,
        //                Message = "Ошибка сервера: " + responce.StatusCode.ToString() + await responce.Content.ReadAsStringAsync()
        //            };
        //            return null;
        //        }
        //        else
        //        {
        //            serverResponce = await responce.Content.ReadFromJsonAsync<ResponseTokenAndStuff>(); 
        //            Client.SetToken(serverResponce.Token);
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = true,
        //                //Message = $"Авторизация прошла успешно! Токен - {serverResponce.Token}"
        //                //$"Авторизация прошла успешно! Добро пожаловать,{authUser.Username}"
        //            };
        //            UserDTO authUser = new UserDTO()
        //            {
        //                Id = serverResponce.UserId,
        //                Username = username,
        //                Password = password,
        //                Email = serverResponce.Email,
        //                ContactPhone = serverResponce.Phone
        //            };
        //            return authUser;
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

        //public async Task UpdateUser(UserDTO user)
        //{
        //    try
        //    {
        //        user.Id = userService.CurrentUser.Id;
        //        string json=JsonSerializer.Serialize(user);
        //        var responce = await client.PutAsync($"Users/{user.Id}", new StringContent(json, Encoding.UTF8, "application/json"));
        //        if (!responce.IsSuccessStatusCode)
        //        {
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = false,
        //                Message = "Ошибка сервера: " + responce.StatusCode.ToString() + await responce.Content.ReadAsStringAsync()
        //            };
        //        }
        //        else
        //        {
                   
        //            CurrentOperationResult = new OperationResult()
        //            {
        //                IsSuccess = true,
        //                Message = $"Данные успешно изменены"
        //            };
        //            userService.SetCurrentUser(user);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CurrentOperationResult = new OperationResult()
        //        {
        //            IsSuccess = false,
        //            Message = "Ошибка: " + ex.Message
        //        };
        //    }

        //}
    }
}
