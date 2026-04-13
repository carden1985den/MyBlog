using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? Picture { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public string FullName()
        {
            return $"{FirstName} {LastName}".Trim();
        }

    }
}
