using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        // Пользователи
        public DbSet<User> Users => Set<User>();
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
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Login = "admin", Password = "123" },
                new User { Id = 2, Login = "user", Password = "User" },
                new User { Id = 3, Login = "guest", Password = "Guest" }
                );
            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile { Id = 1, FirstName = "Admin", LastName = "Admin", UserId = 1 },
                new UserProfile { Id = 2, FirstName = "User", LastName = "User", UserId = 2 },
                new UserProfile { Id = 3, FirstName = "Guest", LastName = "Guest", UserId = 3 }
                );
        }
    }
}
