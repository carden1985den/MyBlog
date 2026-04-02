using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace BLL.Entity
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Post>? Posts { get; set; }

    }
}
