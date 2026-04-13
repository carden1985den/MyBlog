using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string ? LastName { get; set; }
        public string Role { get; set; } = null!;
    }
}
