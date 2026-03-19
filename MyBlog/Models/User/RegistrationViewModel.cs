using System.ComponentModel.DataAnnotations;

namespace WEB.Models.User
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        [DataType(DataType.Text)]
        public string Username { get; set; } = null!;

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [Display(Name = "Имя")]
        [DataType(DataType.Text)] 
        public string Firstname { get; set; } = null!;

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string? Lastname { get; set; } = null!;

        [Display(Name = "Пикча")]
        [DataType(DataType.Text)]
        public string? Pictures { get; set; } = null!;

        public RegistrationViewModel()
        {
            Pictures = "https://cdn-icons-png.flaticon.com/512/149/149071.png";
        }
    }
}
