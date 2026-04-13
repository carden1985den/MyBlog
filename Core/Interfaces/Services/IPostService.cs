using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Services
{
    public interface IPostService
    {
        IEnumerable<Post> GetAll();
        Post? GetById(Guid id);
        void Create(Post item);
        void Update(Post item);
        bool Delete(Post item);
    }
}
