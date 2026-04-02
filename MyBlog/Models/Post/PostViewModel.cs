using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WEB.Models.Post
{
    public class PostViewModel
    {
        [HiddenInput]
        public string? Id { get; set; } 

        [Required]
        [Display(Name = "Название поста")]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "Пост")]
        public string Text { get; set; } = null!;
        public List<string>? SelectedTagId { get; set; }
        public List<TagChekBox>? AvailableTags { get; set; }
    }

    public class TagChekBox
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsChecked { get; set; }
    }
}
