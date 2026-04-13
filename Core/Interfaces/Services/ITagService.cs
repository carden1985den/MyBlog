using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Services
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAll();
        Tag? GetById(Guid id);
        void Create(Tag item);
        void Update(Tag item);
        void Delete(Tag item);
    }
}
