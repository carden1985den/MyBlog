using BLL.Entity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WEB.Models.Post
{
    public class PostViewModel
    {
        [HiddenInput]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Название поста")]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "Пост")]
        public string Text { get; set; } = null!;
    }
}
