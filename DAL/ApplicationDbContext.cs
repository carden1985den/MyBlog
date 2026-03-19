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
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = (RoleEnum)RoleEnum.Admin },
                new Role { Id = 2, RoleName = (RoleEnum)RoleEnum.Editor },
                new Role { Id = 3, RoleName = (RoleEnum)RoleEnum.User }
            );
        }
        
    }
}
