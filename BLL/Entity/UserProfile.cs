using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Entity
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? Picture { get; set; }

        // foregin key and naviogation property to User
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
