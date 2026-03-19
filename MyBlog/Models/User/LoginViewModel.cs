using System.ComponentModel.DataAnnotations;

namespace WEB.Models.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
