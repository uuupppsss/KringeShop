using KringeShopWebClient.Services;
using System.ComponentModel.DataAnnotations;

namespace KringeShopWebClient.Auth.Validation
{
    public class UniquePhoneAttribute: ValidationAttribute
    {
        private bool ifUserUnique;
        private readonly HttpClient _httpClient;
        public UniquePhoneAttribute()
        {
            _httpClient = Client.HttpClient;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string? phone = value as string;
            if (phone != null)
            {
                IfUserExist(phone);
                if (ifUserUnique) return ValidationResult.Success;
                else return new ValidationResult("Этот номер телефона уже занят.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
        private async void IfUserExist(string phone)
        {
            try
            {
                var responce = await _httpClient.GetAsync($"Auth/IfUniquePhone/{phone}");
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
