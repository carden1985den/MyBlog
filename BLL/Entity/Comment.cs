using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BLL.Entity
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime Created { get; set; }
        public Guid? PostId { get; set; }
        public Post Post { get; set; } = null!;
        public Guid? UserId { get; set; }
        public User User { get; set; } = null!;

        public Comment()
        {
            Created = DateTime.Now;
        }


    }
}
