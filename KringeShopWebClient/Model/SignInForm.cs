using KringeShopWebClient.Services;
using System.ComponentModel.DataAnnotations;

namespace KringeShopWebClient.Model
{
    public class SignInForm
    {
        private readonly AuthService connection;
        public SignInForm(AuthService _connection)
        {
            connection = _connection;
        }

        [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 100 символов.")]
        public string Password { get; set; }

    }
}
