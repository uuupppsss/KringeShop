using KringeShopLib.Model;
using KringeShopWebClient.Auth.Validation;
using KringeShopWebClient.Services;
using System.ComponentModel.DataAnnotations;


namespace KringeShopWebClient.Model
{
    public class SignUpForm
    {
        private readonly AuthService connection;
        public SignUpForm(AuthService _connection)
        {
            connection=_connection;
        }

        [Required(ErrorMessage = "Имя обязательно для заполнения.")]
        [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов.")]
        [UniqueUsername]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email обязателен для заполнения.")]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        [UniqueEmail]
        public string Email { get; set; }

        [Required(ErrorMessage = "Номер телефона обязателен для заполнения.")]
        [Phone(ErrorMessage = "Некорректный формат номера телефона.")]
        [UniquePhone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 100 символов.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 100 символов.")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string RepeatPassword { get; set; }

        public async void SignUp()
        {
            await connection.SignUp(new UserDTO
            {
                Username = UserName,
                Password = Password,
                Email = Email,
                ContactPhone = PhoneNumber
            });
        }
    }
}
