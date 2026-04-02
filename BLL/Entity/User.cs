using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entity
{
    public class User
    {
        public  Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserProfile Profile { get; set; } = null!;
        public List<Post>? Post { get; set; }
        public List<Comment>? Comment { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}
