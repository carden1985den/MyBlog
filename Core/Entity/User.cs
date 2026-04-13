using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public class User
    {
        public  Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public virtual UserProfile Profile { get; set; } = null!;
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
    }
}
