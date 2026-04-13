using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Comement
{
    public class CommentViewModel
    {
        [HiddenInput]
        public string Id { get; set; } = null!;

        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; } = null!;

        [HiddenInput]
        public string PostId { get; set; } = null!;
       
    }
}
