using Core.Models.Post;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Tag
{
    public class TagsViewModel
    {
        public List<TagChekBox>? Tags { get; set; }

        [Required]
        [Display(Name = "Имя тэга")]
        public string NewTagName { get; set; } = null!;
    }
}
