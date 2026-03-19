using BLL.Entity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace WEB.Models.Comement
{
    public class CommentViewModel
    {
        [HiddenInput]
        public string? Id { get; set; }

        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; } = null!;

        [HiddenInput]
        public string? PostId { get; set; }
       
    }
}
