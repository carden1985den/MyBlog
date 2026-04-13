using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public TEntity? GetById(Guid id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public void Create(TEntity item)
        {
            _dbContext.Set<TEntity>().Add(item);
            _dbContext.SaveChanges();
        }

        public void Update(TEntity item)
        {
            _dbContext.Set<TEntity>().Update(item);
            _dbContext.SaveChanges();
        }

        public void Delete(TEntity item)
        {
            _dbContext.Set<TEntity>().Remove(item);
            _dbContext.SaveChanges();
        }
    }
}
