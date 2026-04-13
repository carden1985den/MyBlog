using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.User
{
    public class EditViewModel
    {

        [HiddenInput(DisplayValue =false)]
        public string? Id { get; set; }

        [Display(Name = "Логин")]
        [DataType(DataType.Text)]
        public string Username { get; set; } = null!;

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = null!;

        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = null!;

        [Display(Name = "Пикча")]
        [DataType(DataType.Text)]
        public string Picture { get; set; } = null!;
    }
}
