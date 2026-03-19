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
        public Guid PostId { get; set; }
        public List<Post> Post { get; } = new();
    }
}
