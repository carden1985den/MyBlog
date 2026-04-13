using Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public class Role
    {
        public Guid Id { get; set; }
        public RoleEnum Name { get; set; }
    }
}
