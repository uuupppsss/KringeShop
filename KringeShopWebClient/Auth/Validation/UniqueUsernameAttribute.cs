using KringeShopWebClient.Services;
using System.ComponentModel.DataAnnotations;

namespace KringeShopWebClient.Auth.Validation
{
    public class UniqueUsernameAttribute:ValidationAttribute
    {
        private bool ifUserUnique;
        private readonly HttpClient _httpClient;
        public UniqueUsernameAttribute()
        {
            _httpClient = Client.HttpClient;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string? username = value as string;
            if (username != null)
            {
                IfUserExist(username);
                if (ifUserUnique) return ValidationResult.Success;
                else return new ValidationResult("Этот логин уже занят.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
        private async void IfUserExist(string username)
        {
            try
            {
                var responce = await _httpClient.GetAsync($"Auth/IfUniqueUsername/{username}");
                if (!responce.IsSuccessStatusCode) ifUserUnique = false;
                else ifUserUnique = true;
            }
            catch
            {
                ifUserUnique = false;
            }
        }
    }
}
