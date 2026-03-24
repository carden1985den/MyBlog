using BLL.Entity;
using BLL.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        // Пользователи
        public DbSet<User> Users => Set<User>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Tag> Tags => Set<Tag>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userGuid1 = Guid.NewGuid();
            var userGuid2 = Guid.NewGuid();
            var userGuid3 = Guid.NewGuid();

            modelBuilder.Entity<User>().HasData(
                new User() { Id = userGuid1, RoleId = 1, Login = "admin", Password = "123" },
                new User() { Id = userGuid2, RoleId = 2, Login = "editor", Password = "123"},
                new User() { Id = userGuid3, RoleId = 3, Login = "user", Password = "123"}
                );

            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile() { Id = Guid.NewGuid(), FirstName = "Ivan", LastName = "Ivanov", UserId = userGuid1 },
                new UserProfile() { Id = Guid.NewGuid(), FirstName = "Petr", LastName = "Petrov", UserId = userGuid2 },
                new UserProfile() { Id = Guid.NewGuid(), FirstName = "Sidor", LastName = "Sidorov", UserId = userGuid3 }
                );
            
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = (RoleEnum)RoleEnum.Admin },
                new Role { Id = 2, Name = (RoleEnum)RoleEnum.Editor },
                new Role { Id = 3, Name = (RoleEnum)RoleEnum.User }
            );

        }

    }
}
