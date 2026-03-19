using BLL.Entity;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationDbContext _context;

        private Repository<User> _userRepository;
        private Repository<UserProfile> _userProfileRepository;
        private Repository<Post> _postRepository;
        private Repository<Comment> _commentRepository;
        private Repository<Tag> _tagRepository;

        // Реализация ленивой загрузки для репозитория User.
        public Repository<User> Users
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
        public Repository<UserProfile> UserProfiles
        {
            get
            {
                if (_userProfileRepository == null)
                    _userProfileRepository = new Repository<UserProfile>(_context);
                return _userProfileRepository;
            }
        }
        
        public Repository<Post> Posts
        {
            get
            {
                if (_postRepository == null)
                    _postRepository = new Repository<Post>(_context);
                
                return _postRepository;
            }
        }

        public Repository<Comment> Comments
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new Repository<Comment>(_context);
                return _commentRepository;
            }
        }

        public Repository<Tag> Tags
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
            _context = context;
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
