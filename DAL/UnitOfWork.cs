using DAL.Repositories;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Entity;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        private IRepository<User> _userRepository = null!;
        private IRepository<UserProfile> _userProfileRepository = null!;
        private IRepository<Role> _roleRepository = null!;
        private IRepository<Post> _postRepository = null!;
        private IRepository<Comment> _commentRepository = null!;
        private IRepository<Tag> _tagRepository = null!;
        

        // Реализация ленивой загрузки для репозитория User.
        public IRepository<User> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new Repository<User>(_context);
                
                return _userRepository;
            }
        }

        // Реализация ленивой загрузки для репозитория UserProfile.
        // Репозиторий будет создан только при первом обращении к свойству UserProfiles, что позволяет оптимизировать использование ресурсов и улучшить производительность приложения.
        public IRepository<UserProfile> UserProfiles
        {
            get
            {
                if (_userProfileRepository == null)
                    _userProfileRepository = new Repository<UserProfile>(_context);
                return _userProfileRepository;
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new Repository<Role>(_context);

                return _roleRepository;
            }
        }

        public IRepository<Post> Posts
        {
            get
            {
                if (_postRepository == null)
                    _postRepository = new Repository<Post>(_context);
                
                return _postRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new Repository<Comment>(_context);
                return _commentRepository;
            }
        }

        public  IRepository<Tag> Tags
        {
            get
            {
                if (_tagRepository == null)
                    _tagRepository = new Repository<Tag>(_context);

                return _tagRepository;
            }
        }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
