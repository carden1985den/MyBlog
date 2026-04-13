using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Services
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAll();
        Role? GetById(Guid id);
        void Create(Role item);
        void Update(Role item);
        void Delete(Role item);
    }
}
