﻿using BredWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BredWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<Person>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        //public DbSet<UserIdList> UserIdLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<UserIdList>()
            //    .HasKey(uid => new { uid.GroupId, uid.PersonId });
            //builder.Entity<UserIdList>()
            //    .HasOne(uid => uid.Group)
            //    .WithMany(g => g.UserIdList)
            //    .HasForeignKey(uid => uid.GroupId);
            //builder.Entity<UserIdList>()
            //    .HasOne(uid => uid.Person)
            //    .WithMany(p => p.GroupIdList)
            //    .HasForeignKey(uid => uid.PersonId);
        }
    }
}
