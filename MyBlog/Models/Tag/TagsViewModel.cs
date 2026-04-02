using System.ComponentModel.DataAnnotations;
using WEB.Models.Post;

namespace WEB.Models.Tag
{
    public class TagsViewModel
    {
        public List<TagChekBox>? Tags { get; set; }

        [Required]
        [Display(Name = "Имя тэга")]
        public string NewTagName { get; set; } = null!;
    }
}
