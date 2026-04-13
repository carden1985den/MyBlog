using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Services
{
    public interface ICommentService
    {
        IQueryable<Comment> GetAll();
        Comment? GetById(Guid id);
        void Create(Comment item);
        void Update(Comment item);
        void Delete(Comment item);
    }
}