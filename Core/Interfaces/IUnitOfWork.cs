using Core.Entity;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Core
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<UserProfile> UserProfiles { get; }
        IRepository<Role> Roles { get; }
        IRepository<Post> Posts { get; }
        IRepository<Tag> Tags { get; }
        IRepository<Comment> Comments { get; }
        

        void Save();
        new void Dispose();
    }
}
