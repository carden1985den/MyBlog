using System;
using System.Collections;
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
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Comment> Comment { get; set; } = new List<Comment>();
        public Guid TagId { get; set; }
        public ICollection<Tag> Tag { get; set; } = new List<Tag>();
    }
}
