using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Core.Interfaces
{
    public interface IRepository <TEntity> where TEntity : class
    {
        // get all
        IQueryable<TEntity> GetAll ();

        // get by id
        TEntity? GetById(Guid id);

        // create
        void Create(TEntity item);

        // update
        void Update(TEntity item);

        // delete
        void Delete(TEntity item);
    }
}
