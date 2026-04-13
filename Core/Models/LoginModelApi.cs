using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class LoginModelApi
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; } = null!;
    }
}
