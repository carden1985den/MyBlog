using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entity
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime Created { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public List<Comment>? Comment { get; set; }
        public List<Guid>? TagId { get; set; }
        public List<Tag>? Tag { get; set; }
    }
}
