using System.ComponentModel.DataAnnotations;

namespace KringeShopWebClient.Model
{
    public class UserInfoModel
    {
        [Required(ErrorMessage = "Имя обязательно для заполнения.")]
        [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов.")]
        //[UniqueUsername]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email обязателен для заполнения.")]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        //[UniqueEmail]
        public string Email { get; set; }

        [Required(ErrorMessage = "Номер телефона обязателен для заполнения.")]
        [Phone(ErrorMessage = "Некорректный формат номера телефона.")]
        //[UniquePhone]
        public string PhoneNumber { get; set; }


    }
}
