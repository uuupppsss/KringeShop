using KringeShopWebClient.Services;
using System.ComponentModel.DataAnnotations;

namespace KringeShopWebClient.Auth.Validation
{
    public class UniqueEmailAttribute: ValidationAttribute
    {
        private bool ifUserUnique;
        private readonly HttpClient _httpClient;
        public UniqueEmailAttribute()
        {
            _httpClient = Client.HttpClient;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string? email=value as string;
            if (email != null)
            {
                IfUserExist(email);
                if(ifUserUnique) return ValidationResult.Success;
                else return new ValidationResult("Этот email уже занят.");
            }
            else
            {
                return ValidationResult.Success;
            }

        }

        private async void IfUserExist(string email)
        {
            try
            {
                var responce = await _httpClient.GetAsync($"Auth/IfUniqueEmail/{email}");
                if (!responce.IsSuccessStatusCode) ifUserUnique = false;
                else ifUserUnique= true;
            }
            catch
            {
                ifUserUnique = false;
            }
        }
    }
}
